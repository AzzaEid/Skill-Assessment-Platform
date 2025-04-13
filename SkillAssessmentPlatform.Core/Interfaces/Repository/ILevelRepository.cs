using SkillAssessmentPlatform.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface ILevelRepository : IGenericRepository<Level>
    {
        Task<IEnumerable<Level>> GetLevelsByIdAsync(int trackId);
        Task<Level> GetLevelWithStagesAsync(int levelId);
        Task AddAsync(Level level);


        //    Task<IEnumerable<Level>> GetStagesByLevelIdAsync(int trackId);
        //     Task<Level> GetLevelWithStagesAsync(int levelId);


        Task<IEnumerable<Level>> GetLevelsByTrackIdAsync(int trackId);
        Task<Level> GetFirstLevelByTrackIdAsync(int trackId);
    }

}