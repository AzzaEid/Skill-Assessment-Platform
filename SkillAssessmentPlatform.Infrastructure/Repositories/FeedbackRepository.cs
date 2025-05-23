using Microsoft.EntityFrameworkCore;
using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
    {
        private readonly AppDbContext _context;

        public FeedbackRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Feedback>> GetByExaminerIdAsync(string examinerId)
        {
            return await _context.Feedbacks
                .Where(f => f.ExaminerId == examinerId)
                .Include(f => f.DetailedFeedbacks)
                .ToListAsync();
        }

       
    }
}
