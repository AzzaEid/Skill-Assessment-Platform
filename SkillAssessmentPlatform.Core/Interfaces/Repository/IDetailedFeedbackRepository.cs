using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface IDetailedFeedbackRepository : IGenericRepository<DetailedFeedback>
    {
        Task<IEnumerable<DetailedFeedback>> GetByFeedbackIdAsync(int feedbackId);
    }


}
