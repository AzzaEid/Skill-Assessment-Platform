using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Certificates_and_Notifications;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{

    public class ApplicantRepository : GenericRepository<Applicant>, IApplicantRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ApplicantRepository> _logger;

        public ApplicantRepository(
            AppDbContext context,
            UserManager<User> userManager,
            ILogger<ApplicantRepository> logger) : base(context)
        {
            _userManager = userManager;
            _logger = logger;
        }

        // اضطرينا نعمل افررايد لأن الابلكنت موجود في يوزر وفي ابلكنت
        public override async Task<IEnumerable<Applicant>> GetAllAsync()
        {
            return await _context.Users
                .OfType<Applicant>()
                .Where(a => a.IsActive == true)
                .ToListAsync();
        }

        public override IQueryable<Applicant> GetPagedQueryable(int page, int pageSize)
        {
            return _context.Users
                .OfType<Applicant>()
                .Where(a => a.IsActive == true)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public override async Task<Applicant> GetByIdAsync(string id)
        {
            var applicant = await _context.Users
                .OfType<Applicant>()
                .Where(a => a.IsActive == true)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (applicant == null)
                throw new UserNotFoundException($"No applicant with id: {id}");

            return applicant;
        }

        public async Task<Applicant> UpdateStatusAsync(string id, ApplicantStatus status)
        {
            var applicant = await GetByIdAsync(id);
            applicant.Status = status;

            var result = await _userManager.UpdateAsync(applicant);
            if (!result.Succeeded)
            {
                throw new BadRequestException($"Failed to update applicant status", result.Errors);
            }

            return applicant;
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsAsync(string applicantId)
        {
            return await _context.Enrollments
                .Where(e => e.ApplicantId == applicantId)
                .Include(e => e.Track)
                .ToListAsync();
        }

        public async Task<IEnumerable<AppCertificate>> GetCertificatesAsync(string applicantId)
        {
            return await _context.Certificates
                .Where(c => c.ApplicantId == applicantId)
                .Include(c => c.LevelProgress)
                    .ThenInclude(lp => lp.Level)
                .ToListAsync();
        }

        public async Task<Enrollment> EnrollInTrackAsync(string applicantId, int trackId)
        {

            // Check if enrollment already exists
            var existingEnrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.ApplicantId == applicantId && e.TrackId == trackId);

            if (existingEnrollment != null)
                throw new BadRequestException("Applicant is already enrolled in this track");

            // Check if track exists
            var track = await _context.Tracks.FindAsync(trackId);
            if (track == null)
                throw new KeyNotFoundException($"Track with id {trackId} not found");

            // Create enrollment
            var enrollment = new Enrollment
            {
                ApplicantId = applicantId,
                TrackId = trackId,
                EnrollmentDate = DateTime.Now,
                Status = EnrollmentStatus.Active
            };

            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();

            // Get the first level of the track
            var firstLevel = await _context.Levels
                .Where(l => l.TrackId == trackId && l.Order == 1 && l.IsActive)
                .FirstOrDefaultAsync();

            if (firstLevel != null)
            {
                // Create level progress
                var levelProgress = new LevelProgress
                {
                    EnrollmentId = enrollment.Id,
                    LevelId = firstLevel.Id,
                    Status = ProgressStatus.InProgress,
                    StartDate = DateTime.Now
                };

                await _context.LevelProgresses.AddAsync(levelProgress);

                // Get the first stage of the level
                var firstStage = await _context.Stages
                    .Where(s => s.LevelId == firstLevel.Id && s.Order == 1 && s.IsActive)
                    .FirstOrDefaultAsync();

                if (firstStage != null)
                {
                    // Create stage progress
                    var stageProgress = new StageProgress
                    {
                        //EnrollmentId = enrollment.Id,
                        StageId = firstStage.Id,
                        Status = ProgressStatus.InProgress,
                        StartDate = DateTime.Now,
                        Attempts = 1
                    };

                    await _context.StageProgresses.AddAsync(stageProgress);
                }

                await _context.SaveChangesAsync();
            }


            //var enrollment = new Enrollment();
            return enrollment;
        }

        public async Task<IEnumerable<LevelProgress>> GetProgressAsync(string applicantId)
        {
            return await _context.LevelProgresses
                .Include(lp => lp.Level)
                    .ThenInclude(l => l.Track)
                .Include(lp => lp.Enrollment)
                .Where(lp => lp.Enrollment.ApplicantId == applicantId)
                .ToListAsync();
        }
    }

}
