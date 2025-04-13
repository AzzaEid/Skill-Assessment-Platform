using Microsoft.EntityFrameworkCore;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface IExaminerLoadRepository : IGenericRepository <ExaminerLoad>
    {
        Task<IEnumerable<ExaminerLoad>> GetByExaminerIdAsync(string examinerId);
        Task<ExaminerLoad> UpdateWorkLoadAsync(int id, int workLoad);
        Task<bool> CanTakeMoreLoadAsync(string examinerId, StageType type);
         Task<ExaminerLoad> IncrementWorkloadAsync(string examinerId, StageType stageType);
         Task<ExaminerLoad> DecrementWorkloadAsync(string examinerId, StageType stageType);

    }
}
