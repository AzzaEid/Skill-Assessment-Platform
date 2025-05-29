using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class ExaminerLoadRepository : GenericRepository<ExaminerLoad>, IExaminerLoadRepository
    {
        private readonly ILogger<ExaminerLoadRepository> _logger;
        public ExaminerLoadRepository(AppDbContext context,
            ILogger<ExaminerLoadRepository> logger) : base(context)
        {
            _logger = logger;
        }
        public async Task<IEnumerable<ExaminerLoad>> GetByExaminerIdAsync(string examinerId)
        {
            return await _context.ExaminerLoads
                .Where(l => l.ExaminerID == examinerId)
                .ToListAsync();
        }
        public async Task<ExaminerLoad> UpdateWorkLoadAsync(int id, int newWorkLoad)
        {
            var examinerLoad = await _context.ExaminerLoads.FindAsync(id);

            if (examinerLoad == null)
                throw new KeyNotFoundException($"ExaminerLoad with id {id} not found");

            examinerLoad.MaxWorkLoad = newWorkLoad;

            _context.ExaminerLoads.Update(examinerLoad);
            await _context.SaveChangesAsync();

            return examinerLoad;
        }
        public async Task<bool> CanTakeMoreLoadAsync(string examinerId, LoadType type)
        {
            var load = await _context.ExaminerLoads
                .FirstOrDefaultAsync(e => e.ExaminerID == examinerId && e.Type == type);

            if (load == null)
                return false;

            return load.CurrWorkLoad < load.MaxWorkLoad;
        }

        public async Task<ExaminerLoad> IncrementWorkloadAsync(string examinerId, LoadType type)
        {
            var examinerLoad = await _context.ExaminerLoads
                .FirstOrDefaultAsync(el => el.ExaminerID == examinerId && el.Type == type);

            if (examinerLoad == null)
                throw new KeyNotFoundException($"Examiner load for examiner {examinerId} and type {type} not found");

            examinerLoad.CurrWorkLoad += 1;
            _context.ExaminerLoads.Update(examinerLoad);
            await _context.SaveChangesAsync();

            return examinerLoad;
        }

        public async Task<ExaminerLoad>? DecrementWorkloadAsync(string examinerId, LoadType type)
        {
            var examinerLoad = await _context.ExaminerLoads
                .FirstOrDefaultAsync(el => el.ExaminerID == examinerId && el.Type == type);

            if (examinerLoad == null) return null;
            //throw new KeyNotFoundException($"Examiner load for examiner {examinerId} and type {type} not found");

            if (examinerLoad.CurrWorkLoad > 0)
                examinerLoad.CurrWorkLoad -= 1;

            _context.ExaminerLoads.Update(examinerLoad);
            await _context.SaveChangesAsync();

            return examinerLoad;
        }

    }
}

