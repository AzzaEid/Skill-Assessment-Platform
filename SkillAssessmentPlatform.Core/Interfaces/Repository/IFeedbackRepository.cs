using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface IFeedbackRepository : IGenericRepository<Feedback>
    {
        Task<IEnumerable<Feedback>> GetByExaminerIdAsync(string examinerId);
    }
}
