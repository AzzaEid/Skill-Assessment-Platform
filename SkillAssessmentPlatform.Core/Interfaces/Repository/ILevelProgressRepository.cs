using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface ILevelProgressRepository : IGenericRepository<LevelProgress>
    {
        Task<IEnumerable<LevelProgress>> GetByEnrollmentIdAsync(int enrollmentId);
        Task<LevelProgress> GetCurrentLevelProgressAsync(int enrollmentId);
        Task<LevelProgress> UpdateStatusAsync(int levelProgressId, ProgressStatus status);
        Task<LevelProgress> CreateNextLevelProgressAsync(int enrollmentId, int currentLevelId);
        Task<LevelProgress> GetLatestActiveLPAsync(string applicantId);
        Task<IEnumerable<LevelProgress>> GetCompletedLevelsByEnrollmentIdAsync(int enrollmentId);
    }
}
