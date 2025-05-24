using SkillAssessmentPlatform.Core.Entities.TrackLevelStage.SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class AssociatedSkillsRepository : GenericRepository<AssociatedSkill>, IAssociatedSkillsRepository
    {
        public AssociatedSkillsRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
    }
}
