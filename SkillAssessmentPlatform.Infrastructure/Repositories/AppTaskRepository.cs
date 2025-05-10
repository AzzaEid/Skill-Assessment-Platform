using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class AppTaskRepository : GenericRepository<AppTask>, IAppTaskRepository
    {
        private readonly AppDbContext _context;

        public AppTaskRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AppTask>> GetByTaskPoolIdAsync(int taskPoolId)
        {
            return await _context.Tasks.Where(t => t.TaskPoolId == taskPoolId).ToListAsync();
        }
    }

}
