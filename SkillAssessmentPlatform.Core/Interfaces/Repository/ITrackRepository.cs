using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface ITrackRepository : IGenericRepository<Track>
    {

        Task<Track> GetTrackWithDetailsAsync(int id);


        Task<IEnumerable<Track>> GetOnlyActiveTracksAsync();

        Task<IEnumerable<Track>> GetOnlyDeactivatedTracksAsync();

        Task<IEnumerable<Level>> GetLevelsByTrackIdAsync(int trackId);
        Task AddLevelAsync(int trackId, Level level);
        Task<List<Track>> GetByExaminerIdAsync(string examinerId);
        Task<IEnumerable<Track>> GetAllWithDetailsAsync();
        Task<bool> AddLevelToTrackAsync(int trackId, Level level);
        Task<IEnumerable<Stage>> GetStagesByTrackAndTypeAsync(int trackId, StageType type);

    }
}