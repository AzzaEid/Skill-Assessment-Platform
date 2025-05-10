using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using Task = System.Threading.Tasks.Task;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface IExamRepository

    {

        Task AddAsync(Exam exam);
        Task<Exam> GetByStageIdAsync(int stageId);
        Task<Exam> GetByIdAsync(int id);
        Task<bool> SoftDeleteAsync(int id);


    }
}
