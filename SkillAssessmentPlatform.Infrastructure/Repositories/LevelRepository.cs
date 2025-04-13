using Microsoft.EntityFrameworkCore;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class LevelRepository : GenericRepository<Level>, ILevelRepository
    {
        private readonly AppDbContext _context;

        public LevelRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Level>> GetLevelsByIdAsync(int levelId)
        {
            return await _context.Levels
                .Where(l => l.Id == levelId)
                .OrderBy(l => l.Order)
                .ToListAsync();
        }

        public async Task<Level> GetLevelWithStagesAsync(int levelId)
        {
            return await _context.Levels
                .Include(l => l.Stages)
                .FirstOrDefaultAsync(l => l.Id == levelId);
        }

        public async Task AddAsync(Level level)
        {
            await _context.Levels.AddAsync(level);
            _context.SaveChanges();
        }

       
    }

}