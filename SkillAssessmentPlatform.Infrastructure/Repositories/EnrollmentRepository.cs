using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
    {
        private readonly ILogger<EnrollmentRepository> _logger;

        public EnrollmentRepository(
            AppDbContext context,
            ILogger<EnrollmentRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<Enrollment>> GetByApplicantIdAsync(string applicantId, int page = 1, int pageSize = 10)
        {
            return await _context.Enrollments
                .Where(e => e.ApplicantId == applicantId)
                .Include(e => e.Track)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountByApplicantIdAsync(string applicantId)
        {
            return await _context.Enrollments
                .CountAsync(e => e.ApplicantId == applicantId);
        }

        public async Task<Enrollment> GetByApplicantAndTrackAsync(string applicantId, int trackId)
        {
            return await _context.Enrollments
                .Include(e => e.Track)
                .FirstOrDefaultAsync(e => e.ApplicantId == applicantId && e.TrackId == trackId);
        }
        public override async Task<Enrollment> GetByIdAsync(int id)
        {
            return await _context.Enrollments
                .Include(e => e.Track)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Enrollment> UpdateStatusAsync(int enrollmentId, EnrollmentStatus status)
        {
            var enrollment = await _context.Enrollments.FindAsync(enrollmentId);

            if (enrollment == null)
                throw new KeyNotFoundException($"Enrollment with id {enrollmentId} not found");

            enrollment.Status = status;

            _context.Enrollments.Update(enrollment);
            await _context.SaveChangesAsync();

            return enrollment;
        }
        public async Task<Enrollment> GetLatestActiveEnrollmentAsync(string applicantId)
        {
            return await _context.Enrollments
                .Where(e => e.ApplicantId == applicantId && e.Status == EnrollmentStatus.Active)
                .Include(e => e.Track)
                .OrderByDescending(e => e.EnrollmentDate)
                .FirstOrDefaultAsync();
        }
    }
}
