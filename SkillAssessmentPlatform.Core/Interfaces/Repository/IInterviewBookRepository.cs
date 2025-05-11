using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface IInterviewBookRepository : IGenericRepository<InterviewBook>
    {
        Task<IEnumerable<InterviewBook>> GetByApplicantIdAsync(string applicantId);
        Task<IEnumerable<InterviewBook>> GetByExaminerIdAsync(string examinerId);
        Task<IEnumerable<InterviewBook>> GetByStageIdAsync(int stageId);
        Task<InterviewBook> BookInterviewAsync(string applicantId, int interviewId, int appointmentId);
        Task<InterviewBook> UpdateInterviewStatusAsync(int interviewBookId, string status);
        Task<InterviewBook> GenerateMeetingLinkAsync(int interviewBookId);
    }
}
