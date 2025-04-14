using Microsoft.EntityFrameworkCore;
using SkillAssessmentPlatform.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface IStageProgressRepository : IGenericRepository<StageProgress>
    {
        Task<IEnumerable<StageProgress>> GetByEnrollmentIdAsync(int enrollmentId);
        Task<StageProgress> GetCurrentStageProgressAsync(int enrollmentId);
        Task<StageProgress>  UpdateStatusAsync(int stageProgressId, string status, int score = 0);
        Task<StageProgress> AssignExaminerAsync(int stageProgressId, string examinerId);
        Task<StageProgress> CreateNextStageProgressAsync(int enrollmentId, int currentStageId);
        Task<int> GetAttemptCountAsync(int enrollmentId, int stageId);
        Task<StageProgress> CreateNewAttemptAsync(int enrollmentId, int stageId);
        Task<IEnumerable<StageProgress>> GetCompletedStagesByEnrollmentIdAsync(int enrollmentId);

        Task<IEnumerable<StageProgress>> GetFailedStagesByEnrollmentIdAsync(int enrollmentId);




    }
}
