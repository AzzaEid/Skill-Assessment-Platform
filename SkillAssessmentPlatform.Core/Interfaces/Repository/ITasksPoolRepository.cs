using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface ITasksPoolRepository : IGenericRepository<TasksPool>
    {
        Task<TasksPool> GetByStageIdAsync(int stageId);
    }

}
