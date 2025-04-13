using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface IEvaluationCriteriaRepository : IGenericRepository<EvaluationCriteria>
    {
        Task<IEnumerable<EvaluationCriteria>> GetByStageIdAsync(int stageId);

        Task AddAsync(EvaluationCriteria evaluationCriteria);
    }

}