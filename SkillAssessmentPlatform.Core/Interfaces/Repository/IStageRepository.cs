using Microsoft.EntityFrameworkCore;
using SkillAssessmentPlatform.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface IStageRepository : IGenericRepository<Stage>
    {
        Task<IEnumerable<Stage>> GetStagesByLevelIdAsync(int levelId);
        Task<Stage> GetFirstStageByLevelIdAsync(int levelId);
        
        Task AddAsync(Stage stage);

        Task<Stage> GetByIdWithCriteriaAsync(int stageId);

    }

}