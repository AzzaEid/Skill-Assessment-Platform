using Microsoft.EntityFrameworkCore;
using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class EvaluationCriteriaRepository : GenericRepository<EvaluationCriteria>, IEvaluationCriteriaRepository
    {
        private readonly AppDbContext _context;

        public EvaluationCriteriaRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EvaluationCriteria>> GetByStageIdAsync(int stageId)
        {
            return await _context.EvaluationCriteria
                .Where(c => c.StageId == stageId)
                .ToListAsync();
        }
    }

}