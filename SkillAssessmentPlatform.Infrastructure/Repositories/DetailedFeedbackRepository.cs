using Microsoft.EntityFrameworkCore;
using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class DetailedFeedbackRepository : GenericRepository<DetailedFeedback>, IDetailedFeedbackRepository
    {
        private readonly AppDbContext _context;

        public DetailedFeedbackRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DetailedFeedback>> GetByFeedbackIdAsync(int feedbackId)
        {
            return await _context.DetailedFeedbacks
                .Where(df => df.FeedbackId == feedbackId)
                .Include(d => d.EvaluationCriteria)
                .ToListAsync();
        }
    }


}
