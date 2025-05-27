using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface IExamRequestRepository : IGenericRepository<ExamRequest>
    {
        Task<IEnumerable<ExamRequest>> GetByApplicantIdAsync(string applicantId);
        Task<IEnumerable<ExamRequest>> GetPendingRequestsByStageIdAsync(int stageId);
        Task<ExamRequest> UpdateStatusAsync(int requestId, ExamRequestStatus status, string instructions = null, DateTime? scheduledDate = null);
        Task<Dictionary<int, int>> GetPendingExamRequestCountsByStageAsync(string trackId);
        Task<IEnumerable<ExamRequest>> GetByStageIdAsync(int stageId);
        Task<ExamRequest> GetWithApplicantAndExamAsync(int requestId);
        Task<ExamRequest> GetByStageProgressIdAsync(int stageProgressId);
        Task<ExamRequest> GetCompletedPendingReviewByApplicantAsync(string applicantId);
    }
}
