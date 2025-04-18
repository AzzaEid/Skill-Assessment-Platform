using Microsoft.EntityFrameworkCore;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Interfaces;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class TrackRepository : GenericRepository<Track>, ITrackRepository

    {

        public TrackRepository(AppDbContext context) : base(context)
        {
            _context = context; 
        }

        public async Task<Track> GetTrackWithDetailsAsync(int trackId)
        {
            return await _context.Tracks
                .Include(t => t.Levels)
                    .ThenInclude(l => l.Stages)
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
      
    }
}