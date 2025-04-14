using SkillAssessmentPlatform.Core.Entities;
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
        Task<LevelProgress> UpdateStatusAsync(int levelProgressId, string status);
        Task<LevelProgress> CreateNextLevelProgressAsync(int enrollmentId, int currentLevelId);
    }
}
