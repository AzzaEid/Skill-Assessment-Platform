using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface IExaminerRepository : IGenericRepository<Examiner>
    {
        Task<Examiner> UpdateSpecializationAsync(string id, string specialization);
        Task<IEnumerable<Track>> GetTracksAsync(string examinerId);
        Task AddTrackToExaminerAsync(string examinerId, int trackId);
        Task<IEnumerable<ExaminerLoad>> GetWorkloadAsync(string examinerId);
        Task<bool> RemoveTrackFromExaminerAsync(string examinerId, int trackId);
        Task<string?> GetAvailableExaminerAsync(StageType stageType);

    }
}
