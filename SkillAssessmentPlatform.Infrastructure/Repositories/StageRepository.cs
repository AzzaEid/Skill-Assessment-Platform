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
    public class StageRepository : GenericRepository<Stage>, IStageRepository
    {
        private readonly AppDbContext _context;

        public StageRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Stage>> GetStagesByLevelIdAsync(int levelId)
        {
            return await _context.Stages
                .Where(s => s.LevelId == levelId)
                .OrderBy(s => s.Order)
                .ToListAsync();
        }

        public async Task AddAsync(Stage stage)
        {
            await _context.Stages.AddAsync(stage);
            _context.SaveChanges();
        }

        Task<IEnumerable<Stage>> IStageRepository.GetStagesByLevelIdAsync(int levelId)
        {
            throw new NotImplementedException();
        }
    }

}