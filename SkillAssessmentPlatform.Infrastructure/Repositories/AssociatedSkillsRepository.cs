using Microsoft.EntityFrameworkCore;
using SkillAssessmentPlatform.Core.Entities.TrackLevelStage;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class AssociatedSkillsRepository : GenericRepository<AssociatedSkill>, IAssociatedSkillsRepository
    {
        public AssociatedSkillsRepository(AppDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<IEnumerable<AssociatedSkill>> GetByTrackIdAsync(int trackId)
        {
            return await _context.AssociatedSkills
                .Where(a => a.TrackId == trackId)
                .ToListAsync();
        }
    }
}
