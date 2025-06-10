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
        public override async Task<Track> GetByIdAsync(int id)
        {
            return await _context.Tracks
                     .Include(t => t.SeniorExaminer)
                     .FirstOrDefaultAsync(t => t.Id == id);

        }
        public async Task<Track> GetTrackWithDetailsAsync(int trackId)
        {
            var track = await _context.Tracks
                .Include(t => t.Levels)
                    .ThenInclude(l => l.Stages)
                        .ThenInclude(s => s.Interview)
                .Include(t => t.Levels)
                    .ThenInclude(l => l.Stages)
                        .ThenInclude(s => s.Exam)
                .Include(t => t.Levels)
                    .ThenInclude(l => l.Stages)
                        .ThenInclude(s => s.TasksPool)
                .Include(t => t.Levels)
                    .ThenInclude(l => l.Stages)
                        .ThenInclude(s => s.EvaluationCriteria)
                .Include(t => t.AssociatedSkills)
                .FirstOrDefaultAsync(t => t.Id == trackId);

            if (track != null)
            {
                track.Levels = track.Levels
                    .OrderBy(l => l.Order)
                    .ToList();

                foreach (var level in track.Levels)
                {
                    level.Stages = level.Stages
                        .OrderBy(s => s.Order)
                        .ToList();
                }
            }
            return track;
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
                .Include(t => t.AssociatedSkills)
                .ToListAsync();
        }


        public async Task<IEnumerable<Track>> GetOnlyDeactivatedTracksAsync()
        {
            return await _context.Tracks
                .Where(t => !t.IsActive)
                   .Include(t => t.AssociatedSkills)
                .ToListAsync();
        }
        public async Task<IEnumerable<Stage>> GetStagesByTrackAndTypeAsync(int trackId, StageType type)
        {
            return await _context.Tracks
                .Where(t => t.Id == trackId)
                .SelectMany(t => t.Levels)
                .SelectMany(l => l.Stages)
                .Where(s => s.Type == type)
                .ToListAsync();
        }

        public async Task<IEnumerable<Track>> GetAllWithDetailsAsync()
        {
            return await _context.Tracks
                .Include(t => t.Levels)
                    .ThenInclude(l => l.Stages)
                    .Include(t => t.AssociatedSkills)

                //.Where(t => t.IsActive)
                .ToListAsync();
        }
        public async Task<IEnumerable<Track>> GetBySeniorIdAsync(string seniorId)
        {
            return await _context.Tracks
                .Where(t => t.SeniorExaminerID == seniorId && t.IsActive)
                .ToListAsync();
        }



        public async Task<bool> AddLevelToTrackAsync(int trackId, Level level)
        {
            var track = await _context.Tracks.Include(t => t.Levels).FirstOrDefaultAsync(t => t.Id == trackId);
            if (track == null) return false;

            level.TrackId = trackId;
            track.Levels.Add(level);

            await _context.SaveChangesAsync();
            return true;
        }

    }
}