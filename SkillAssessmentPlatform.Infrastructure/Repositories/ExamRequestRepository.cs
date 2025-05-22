using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class ExamRequestRepository : GenericRepository<ExamRequest>, IExamRequestRepository
    {
        private readonly ILogger<ExamRequestRepository> _logger;

        public ExamRequestRepository(
            AppDbContext context,
            ILogger<ExamRequestRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<ExamRequest>> GetByApplicantIdAsync(string applicantId)
        {
            return await _context.ExamRequests
                .Where(er => er.ApplicantId == applicantId)
                .Include(er => er.Exam)
                .ThenInclude(e => e.Stage)
                .OrderByDescending(er => er.ScheduledDate)
                .ToListAsync();
        }

        public async Task<ExamRequest> GetWithApplicantAndExamAsync(int requestId)
        {
            return await _context.ExamRequests
                .Include(er => er.Applicant)
                .Include(er => er.Exam)
                    .ThenInclude(e => e.Stage)
                .FirstOrDefaultAsync(er => er.Id == requestId);
        }

        public async Task<IEnumerable<ExamRequest>> GetPendingRequestsByStageIdAsync(int stageId)
        {
            return await _context.ExamRequests
                .Where(er => er.Exam.StageId == stageId && er.Status == ExamRequestStatus.Pending)
                .Include(er => er.Applicant)
                .Include(er => er.Exam)
                .OrderByDescending(er => er.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ExamRequest>> GetByStageIdAsync(int stageId)
        {
            return await _context.ExamRequests
                .Where(er => er.Exam.StageId == stageId)
                .Include(er => er.Applicant)
                .Include(er => er.Exam)
                .OrderByDescending(er => er.ScheduledDate)
                .ToListAsync();
        }

        public async Task<Dictionary<int, int>> GetPendingExamRequestCountsByStageAsync(string trackId)
        {
            // Get the track with its levels and stages
            var track = await _context.Tracks
                .Include(t => t.Levels)
                    .ThenInclude(l => l.Stages)
                .FirstOrDefaultAsync(t => t.Id.ToString() == trackId);

            if (track == null)
            {
                return new Dictionary<int, int>();
            }

            // Get all stage IDs from this track
            var stageIds = track.Levels
                .SelectMany(l => l.Stages)
                .Where(s => s.Type == StageType.Exam && s.IsActive)
                .Select(s => s.Id)
                .ToList();

            // Query to count pending exam requests for each stage
            var pendingCounts = await _context.ExamRequests
                .Where(er => er.Status == ExamRequestStatus.Pending)
                .Include(er => er.Exam)
                .Where(er => stageIds.Contains(er.Exam.StageId))
                .GroupBy(er => er.Exam.StageId)
                .Select(g => new
                {
                    StageId = g.Key,
                    Count = g.Count()
                })
                .ToDictionaryAsync(x => x.StageId, x => x.Count);

            return pendingCounts;
        }

        public async Task<ExamRequest> UpdateStatusAsync(int requestId, ExamRequestStatus status, string instructions = null, DateTime? scheduledDate = null)
        {
            var examRequest = await _context.ExamRequests.FindAsync(requestId);

            if (examRequest == null)
                throw new KeyNotFoundException($"ExamRequest with id {requestId} not found");

            examRequest.Status = status;

            if (instructions != null)
                examRequest.Instructions = instructions;

            if (scheduledDate.HasValue)
                examRequest.ScheduledDate = scheduledDate.Value;

            await _context.SaveChangesAsync();
            return examRequest;
        }
    }
}
