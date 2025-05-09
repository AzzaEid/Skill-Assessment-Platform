using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Users;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface ISeniorRepository
    {
        public Task<List<Examiner>> GetSeniorListAsync();
        public Task<bool> AssignSeniorToTrackAsync(Examiner examiner, Track track);
        public Task<bool> RemoveSeniorFromTrackAsync(Track track);
        public Task<bool> ChangeTrackSeniorAsync(Examiner examiner, Track track);
        public Task<Examiner?> GetSeniorByTrackIdAsync(int trackId);
    }
}
