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
        // عمليات الـ Track
        Task<IEnumerable<Track>> GetAllAsync();
        Task<Track> GetByIdAsync(int id);
        Task<Track> GetTrackWithDetailsAsync(int id);
        Task AddAsync(Level level);

        Task AddAsync(Track track);
      //  Task AddAsync(Level level);

        Task UpdateAsync(Track track);
        Task DeleteAsync(int id);

        Task<IEnumerable<Level>> GetLevelsByTrackIdAsync(int trackId);
        Task AddLevelAsync(int trackId, Level level);

        Task AssignExaminerAsync(int trackId, string examinerId);
        Task RemoveExaminerAsync(int trackId, string examinerId);
        Task<Track> GetTrackWithLevelsAsync(int trackId);
        Task<List<Track>> GetByExaminerIdAsync(string examinerId);
    }
}