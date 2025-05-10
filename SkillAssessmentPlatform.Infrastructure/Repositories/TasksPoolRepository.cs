using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class TasksPoolRepository : GenericRepository<TasksPool>, ITasksPoolRepository
    {
        private readonly AppDbContext _context;

        public TasksPoolRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<TasksPool> GetByStageIdAsync(int stageId)
        {
            return await _context.TasksPools.FirstOrDefaultAsync(x => x.StageId == stageId && x.Stage.IsActive);
        }
    }

}
