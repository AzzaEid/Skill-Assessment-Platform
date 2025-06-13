using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface IStageProgressRepository : IGenericRepository<StageProgress>
    {
        Task<IEnumerable<StageProgress>> GetByLevelProgressIdAsync(int levelProgressId);
        Task<StageProgress> GetCurrentStageProgressAsync(int levelProgressId);
        Task<StageProgress> GetCurrentStageProgressByEnrollmentAsync(int enrollmentId);
        Task<StageProgress> UpdateStatusAsync(int stageProgressId, ProgressStatus status, int score = 0);
        Task<StageProgress> AssignExaminerAsync(int stageProgressId, string examinerId);
        Task<StageProgress> CreateNextStageProgressAsync(int levelProgressId, int currentStageId, string freeExaminerId);
        Task<int> GetAttemptCountAsync(int stageId);
        Task<int> GetLevelProgressIdofStageAsync(int stageId);
        Task<StageProgress> CreateNewAttemptAsync(int enrollmentId, int stageId, string freeExaminerId);
        Task<IEnumerable<StageProgress>> GetCompletedStagesLPIdAsync(int enrollmentId);
        Task<StageProgress> GetLatestSPinLPAsync(int levelProgressId);
        Task<IEnumerable<StageProgress>> GetFailedStagesByEnrollmentIdAsync(int enrollmentId);
        Task<StageProgress> GetByApplicantAndStageAsync(string applicantId, int stageId);
        Task<StageProgress> GetDetailedStageProgressAsync(int stageProgressId);
        Task<IEnumerable<StageProgress>> GetDetailedByLevelProgressIdAsync(int levelProgressId);
        Task<IEnumerable<StageProgress>> GetPendingByExaminerIdAsync(string examinerId);
        Task<IEnumerable<StageProgress>> GetByPendingExaminerIdAndTypeAsync(string examinerId, StageType stageType);

    }
}
