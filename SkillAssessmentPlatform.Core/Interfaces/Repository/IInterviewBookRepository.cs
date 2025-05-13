using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface IInterviewBookRepository : IGenericRepository<InterviewBook>
    {
        Task<IEnumerable<InterviewBook>> GetByApplicantIdAsync(string applicantId);
        Task<IEnumerable<InterviewBook>> GetByExaminerIdAsync(string examinerId);
        Task<IEnumerable<InterviewBook>> GetByStageIdAsync(int stageId);
        Task<InterviewBook> CreateInterviewBookAsync(InterviewBook interviewBook);
        Task<InterviewBook> UpdateInterviewStatusAsync(int interviewBookId, InterviewStatus status);
        Task<InterviewBook> GenerateMeetingLinkAsync(int interviewBookId);
    }
}
