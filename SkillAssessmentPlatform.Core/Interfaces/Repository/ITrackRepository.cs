using SkillAssessmentPlatform.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface ITrackRepository
    {
        Task<IEnumerable<Track>> GetAllAsync();
        Task<Track> GetByIdAsync(int id);
        Task<Track> GetTrackWithDetailsAsync(int id);
     

        Task AddAsync(Track track);

     

        Task<IEnumerable<Level>> GetLevelsByTrackIdAsync(int trackId);
        Task AddLevelAsync(int trackId, Level level);

    
    }
}