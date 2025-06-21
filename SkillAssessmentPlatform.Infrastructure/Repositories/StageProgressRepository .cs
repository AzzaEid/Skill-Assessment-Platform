using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class StageProgressRepository : GenericRepository<StageProgress>, IStageProgressRepository
    {
        private readonly ILogger<StageProgressRepository> _logger;

        public StageProgressRepository(
            AppDbContext context,
            ILogger<StageProgressRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<StageProgress>> GetByLevelProgressIdAsync(int levelProgressId)
        {
            return await _context.StageProgresses
                .Where(sp => sp.LevelProgressId == levelProgressId)
                .Include(sp => sp.Stage)
                .ToListAsync();
        }

        public async Task<StageProgress> GetCurrentStageProgressAsync(int levelProgressId)
        {
            return await _context.StageProgresses
                .Where(sp => sp.LevelProgressId == levelProgressId && sp.Status == ProgressStatus.InProgress)
                .Include(sp => sp.Stage)
                .ThenInclude(s => s.Level)
                .FirstOrDefaultAsync();
        }
        public async Task<StageProgress> GetCurrentStageProgressByEnrollmentAsync(int enrollmentId)
        {
            return await _context.StageProgresses
                .Where(sp => sp.LevelProgress.EnrollmentId == enrollmentId && sp.Status == ProgressStatus.InProgress)
                .Include(sp => sp.Stage)
                .FirstOrDefaultAsync();
        }

        public async Task<StageProgress> UpdateStatusAsync(int stageProgressId, ProgressStatus status, int score = 0)
        {
            var stageProgress = await _context.StageProgresses.FindAsync(stageProgressId);

            if (stageProgress == null)
                throw new KeyNotFoundException($"StageProgress with id {stageProgressId} not found");

            stageProgress.Status = status;
            stageProgress.Score = score;

            if (status == ProgressStatus.Successful || status == ProgressStatus.Failed)
                stageProgress.CompletionDate = DateTime.UtcNow;

            _context.StageProgresses.Update(stageProgress);
            await _context.SaveChangesAsync();

            return stageProgress;
        }

        public async Task<StageProgress> AssignExaminerAsync(int stageProgressId, string examinerId)
        {
            var stageProgress = await _context.StageProgresses.FindAsync(stageProgressId);

            if (stageProgress == null)
                throw new KeyNotFoundException($"StageProgress with id {stageProgressId} not found");

            stageProgress.ExaminerId = examinerId;

            _context.StageProgresses.Update(stageProgress);
            await _context.SaveChangesAsync();

            return stageProgress;
        }

        public async Task<StageProgress> CreateNextStageProgressAsync(int levelProgressId, int nextStageId, string freeExaminerId)
        {

            // Create progress for the next stage
            var stageProgress = new StageProgress
            {
                LevelProgressId = levelProgressId,
                StageId = nextStageId,
                Status = ProgressStatus.InProgress,
                StartDate = DateTime.Now,
                Attempts = 1,
                ExaminerId = freeExaminerId
            };

            await _context.StageProgresses.AddAsync(stageProgress);
            await _context.SaveChangesAsync();

            return stageProgress;
        }



        public async Task<int> GetAttemptCountAsync(int stageId)
        {
            return await _context.StageProgresses
                .CountAsync(sp => sp.StageId == stageId);
        }
        public async Task<int> GetLevelProgressIdofStageAsync(int stageId)
        {
            var sp = await _context.StageProgresses
               .Where(sp => sp.StageId == stageId).FirstAsync();
            return sp.LevelProgressId;
        }
        public async Task<StageProgress> CreateNewAttemptAsync(int levelProgressId, int stageId, string freeExaminerId)
        {


            var attemptCount = await GetAttemptCountAsync(stageId);

            var stageProgress = new StageProgress
            {
                LevelProgressId = levelProgressId,
                StageId = stageId,
                Status = ProgressStatus.InProgress,
                StartDate = DateTime.Now,
                Attempts = attemptCount + 1,
                ExaminerId = freeExaminerId
            };

            await _context.StageProgresses.AddAsync(stageProgress);
            await _context.SaveChangesAsync();

            return stageProgress;
        }
        public async Task<IEnumerable<StageProgress>> GetCompletedStagesLPIdAsync(int levelProgress)
        {
            return await _context.StageProgresses
                .Where(sp => sp.LevelProgressId == levelProgress && sp.Status == ProgressStatus.Successful)
                .Include(sp => sp.Stage)
                .ToListAsync();
        }

        public async Task<IEnumerable<StageProgress>> GetFailedStagesByEnrollmentIdAsync(int enrollmentId)
        {
            return await _context.StageProgresses
                .Where(sp => sp.LevelProgress.EnrollmentId == enrollmentId && sp.Status == ProgressStatus.Failed)
                .Include(sp => sp.Stage)
                .ToListAsync();
        }
        public async Task<StageProgress> GetLatestSPinLPAsync(int levelProgressId)
        {
            return await _context.StageProgresses
                .Where(sp => sp.LevelProgressId == levelProgressId)
                .Include(sp => sp.Stage)
                    .ThenInclude(s => s.Level)
                .OrderByDescending(e => e.StartDate)
                .FirstOrDefaultAsync();
        }

        public async Task<StageProgress> GetByApplicantAndStageAsync(string applicantId, int stageId)
        {
            return await _context.StageProgresses
                .Where(sp => sp.StageId == stageId
                        && sp.LevelProgress.Enrollment.ApplicantId == applicantId)
                .Include(sp => sp.Stage)
                .OrderByDescending(e => e.StartDate)
                .FirstOrDefaultAsync();
        }

        public async Task<StageProgress> GetDetailedStageProgressAsync(int stageProgressId)
        {
            return await _context.StageProgresses
                .Where(sp => sp.Id == stageProgressId)
                .Include(sp => sp.Stage)
                    .ThenInclude(s => s.Exam)
                .Include(sp => sp.Stage)
                    .ThenInclude(s => s.Interview)
                .Include(sp => sp.Stage)
                    .ThenInclude(s => s.TasksPool)
                        .ThenInclude(tp => tp.Tasks)
                .Include(sp => sp.LevelProgress)
                    .ThenInclude(lp => lp.Enrollment)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<StageProgress>> GetDetailedByLevelProgressIdAsync(int levelProgressId)
        {
            return await _context.StageProgresses
                .Where(sp => sp.LevelProgressId == levelProgressId)
                .Include(sp => sp.Stage)
                    .ThenInclude(s => s.Exam)
                .Include(sp => sp.Stage)
                    .ThenInclude(s => s.Interview)
                .Include(sp => sp.Stage)
                    .ThenInclude(s => s.TasksPool)
                        .ThenInclude(tp => tp.Tasks)
                .Include(sp => sp.LevelProgress)
                    .ThenInclude(lp => lp.Enrollment)
                .OrderBy(sp => sp.Stage.Order)
                .ToListAsync();
        }


        public async Task<IEnumerable<StageProgress>> GetPendingByExaminerIdAsync(string examinerId)
        {
            return await _context.StageProgresses
                .Where(sp => sp.ExaminerId == examinerId && sp.Status == ProgressStatus.InProgress)
                .Include(sp => sp.Stage)
                .Include(sp => sp.LevelProgress)
                    .ThenInclude(lp => lp.Enrollment)
                        .ThenInclude(e => e.Track)
                .ToListAsync();
        }
        public async Task<IEnumerable<StageProgress>> GetByPendingExaminerIdAndTypeAsync(string examinerId, StageType stageType)
        {
            return await _context.StageProgresses
                .Where(sp => sp.ExaminerId == examinerId
                        && sp.Status == ProgressStatus.InProgress
                        && sp.Stage.Type == stageType)
                .Include(sp => sp.Stage)
                .Include(sp => sp.LevelProgress)
                    .ThenInclude(lp => lp.Enrollment)
                        .ThenInclude(e => e.Track)
                .ToListAsync();
        }

    }
}