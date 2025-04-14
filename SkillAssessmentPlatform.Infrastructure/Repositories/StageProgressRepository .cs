using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<StageProgress>> GetByEnrollmentIdAsync(int enrollmentId)
        {
            return await _context.StageProgresses
                .Where(sp => sp.EnrollmentId == enrollmentId)
                .Include(sp => sp.Stage)
                .ToListAsync();
        }

        public async Task<StageProgress> GetCurrentStageProgressAsync(int enrollmentId)
        {
            return await _context.StageProgresses
                .Where(sp => sp.EnrollmentId == enrollmentId && sp.Status == "InProgress")
                .Include(sp => sp.Stage)
                .FirstOrDefaultAsync();
        }

        public async Task<StageProgress> UpdateStatusAsync(int stageProgressId, string status, int score = 0)
        {
            var stageProgress = await _context.StageProgresses.FindAsync(stageProgressId);

            if (stageProgress == null)
                throw new KeyNotFoundException($"StageProgress with id {stageProgressId} not found");

            stageProgress.Status = status;
            stageProgress.Score = score;

            if (status == "Successful" || status == "Failed")
                stageProgress.CompletionDate = DateTime.Now;

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

        public async Task<StageProgress> CreateNextStageProgressAsync(int enrollmentId, int currentStageId)
        {
            // Get the current stage
            var currentStage = await _context.Stages.FindAsync(currentStageId);
            if (currentStage == null)
                throw new KeyNotFoundException($"Stage with id {currentStageId} not found");

            // Get the next stage
            var nextStage = await _context.Stages
                .Where(s => s.LevelId == currentStage.LevelId && s.Order == currentStage.Order + 1 && s.IsActive)
                .FirstOrDefaultAsync();

            if (nextStage == null)
                return null; // No next stage, level completed

            // Create progress for the next stage
            var stageProgress = new StageProgress
            {
                EnrollmentId = enrollmentId,
                StageId = nextStage.Id,
                Status = "InProgress",
                StartDate = DateTime.Now,
                Attempts = 1
            };

            await _context.StageProgresses.AddAsync(stageProgress);
            await _context.SaveChangesAsync();

            return stageProgress;
        }

        public async Task<int> GetAttemptCountAsync(int enrollmentId, int stageId)
        {
            return await _context.StageProgresses
                .CountAsync(sp => sp.EnrollmentId == enrollmentId && sp.StageId == stageId);
        }

        public async Task<StageProgress> CreateNewAttemptAsync(int enrollmentId, int stageId)
        {
            // Check if there is an existing attempt in progress
            var existingAttempt = await _context.StageProgresses
                .FirstOrDefaultAsync(sp => sp.EnrollmentId == enrollmentId && sp.StageId == stageId && sp.Status == "InProgress");

            if (existingAttempt != null)
                throw new BadRequestException("There is already an attempt in progress for this stage");

            var attemptCount = await GetAttemptCountAsync(enrollmentId, stageId);

            var stageProgress = new StageProgress
            {
                EnrollmentId = enrollmentId,
                StageId = stageId,
                Status = "InProgress",
                StartDate = DateTime.Now,
                Attempts = attemptCount + 1
            };

            await _context.StageProgresses.AddAsync(stageProgress);
            await _context.SaveChangesAsync();

            return stageProgress;
        }
        public async Task<IEnumerable<StageProgress>> GetCompletedStagesByEnrollmentIdAsync(int enrollmentId)
        {
            return await _context.StageProgresses
                .Where(sp => sp.EnrollmentId == enrollmentId && sp.Status == "Successful")
                .Include(sp => sp.Stage)
                .ToListAsync();
        }

        public async Task<IEnumerable<StageProgress>> GetFailedStagesByEnrollmentIdAsync(int enrollmentId)
        {
            return await _context.StageProgresses
                .Where(sp => sp.EnrollmentId == enrollmentId && sp.Status == "Failed")
                .Include(sp => sp.Stage)
                .ToListAsync();
        }
    }
}
    