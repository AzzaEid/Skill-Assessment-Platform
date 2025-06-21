using SkillAssessmentPlatform.Core.Entities;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface IStageRepository : IGenericRepository<Stage>
    {
        Task<IEnumerable<Stage>> GetStagesByLevelIdAsync(int levelId);
        Task<Stage> GetFirstStageByLevelIdAsync(int levelId);

        Task AddAsync(Stage stage);
        Task<Stage> GetByInterviewId(int interviewId);
        Task<Stage> GetByIdWithCriteriaAsync(int stageId);
        Task<Stage?> GetNextStageInLevelAsync(int currentStageId);
        Task<IEnumerable<Stage>> GetTaskStagesByTrackIdAsync(int trackId);


    }

}