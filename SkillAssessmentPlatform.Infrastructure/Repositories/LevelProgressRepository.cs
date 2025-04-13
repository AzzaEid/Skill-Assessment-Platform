using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class LevelProgressRepository : GenericRepository<LevelProgress>, ILevelProgressRepository
    {
        private readonly ILogger<LevelProgressRepository> _logger;

        public LevelProgressRepository(
            AppDbContext context,
            ILogger<LevelProgressRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<LevelProgress>> GetByEnrollmentIdAsync(int enrollmentId)
        {
            return await _context.LevelProgresses
                .Where(lp => lp.EnrollmentId == enrollmentId)
                .Include(lp => lp.Level)
                .ToListAsync();
        }

        public async Task<LevelProgress> GetCurrentLevelProgressAsync(int enrollmentId)
        {
            return await _context.LevelProgresses
                .Where(lp => lp.EnrollmentId == enrollmentId && lp.Status == "InProgress")
                .Include(lp => lp.Level)
                .FirstOrDefaultAsync();
        }

        public async Task<LevelProgress> UpdateStatusAsync(int levelProgressId, string status)
        {
            var levelProgress = await _context.LevelProgresses.FindAsync(levelProgressId);

            if (levelProgress == null)
                throw new KeyNotFoundException($"LevelProgress with id {levelProgressId} not found");

            levelProgress.Status = status;

            if (status == "Successful" || status == "Failed")
                levelProgress.CompletionDate = DateTime.Now;

            _context.LevelProgresses.Update(levelProgress);
            await _context.SaveChangesAsync();

            return levelProgress;
        }

        public async Task<LevelProgress> CreateNextLevelProgressAsync(int enrollmentId, int currentLevelId)
        {
            // Get the current level
            var currentLevel = await _context.Levels.FindAsync(currentLevelId);
            if (currentLevel == null)
                throw new KeyNotFoundException($"Level with id {currentLevelId} not found");

            // Get the next level
            var nextLevel = await _context.Levels
                .Where(l => l.TrackId == currentLevel.TrackId && l.Order == currentLevel.Order + 1 && l.IsActive)
                .FirstOrDefaultAsync();

            if (nextLevel == null)
                return null; // No next level, track completed

            // Create progress for the next level
            var levelProgress = new LevelProgress
            {
                EnrollmentId = enrollmentId,
                LevelId = nextLevel.Id,
                Status = "InProgress",
                StartDate = DateTime.Now
            };

            await _context.LevelProgresses.AddAsync(levelProgress);
            await _context.SaveChangesAsync();

            // Get first stage of the next level
            var firstStage = await _context.Stages
                .Where(s => s.LevelId == nextLevel.Id && s.Order == 1 && s.IsActive)
                .FirstOrDefaultAsync();

            if (firstStage != null)
            {
                // Create progress for the first stage
                var stageProgress = new StageProgress
                {
                    EnrollmentId = enrollmentId,
                    StageId = firstStage.Id,
                    Status = "InProgress",
                    StartDate = DateTime.Now,
                    Attempts = 1
                };

                await _context.StageProgresses.AddAsync(stageProgress);
                await _context.SaveChangesAsync();
            }

            return levelProgress;
        }
    }
}
