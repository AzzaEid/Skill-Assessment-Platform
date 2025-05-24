using Microsoft.EntityFrameworkCore;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class TrackRepository : GenericRepository<Track>, ITrackRepository

    {

        public TrackRepository(AppDbContext context) : base(context)
        {
            //_context = context; 
        }

        public async Task<Track> GetTrackWithDetailsAsync(int trackId)
        {
            return await _context.Tracks
            .Include(t => t.Levels)
            .ThenInclude(l => l.Stages)
            .ThenInclude(s => s.EvaluationCriteria)
             .FirstOrDefaultAsync(t => t.Id == trackId);

        }

        public async Task<IEnumerable<Level>> GetLevelsByTrackIdAsync(int trackId)
        {
            return await _context.Levels
                .Where(l => l.TrackId == trackId)
                .ToListAsync();
        }

        public async Task AddLevelAsync(int trackId, Level level)
        {
            level.TrackId = trackId;
            await _context.Levels.AddAsync(level);
            await _context.SaveChangesAsync();
        }



        public async Task AddAsync(Track track)
        {
            await _context.Tracks.AddAsync(track);
            _context.SaveChanges();
        }
        public async Task<List<Track>> GetByExaminerIdAsync(string examinerId)
        {
            return await _context.Tracks
                .Where(t => t.Examiners.Any(e => e.Id == examinerId))  // the relation is M-M
                .ToListAsync();
        }

        public async Task<IEnumerable<Track>> GetOnlyActiveTracksAsync()
        {
            return await _context.Tracks
                .Where(t => t.IsActive)
                .ToListAsync();
        }


        public async Task<IEnumerable<Track>> GetOnlyDeactivatedTracksAsync()
        {
            return await _context.Tracks
                .Where(t => !t.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Track>> GetAllWithDetailsAsync()
        {
            return await _context.Tracks
                .Include(t => t.Levels)
                    .ThenInclude(l => l.Stages)
                .Where(t => t.IsActive)
                .ToListAsync();
        }
      


    }
}