using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface IExaminerLoadRepository : IGenericRepository<ExaminerLoad>
    {
        Task<IEnumerable<ExaminerLoad>> GetByExaminerIdAsync(string examinerId);
        Task<ExaminerLoad> UpdateWorkLoadAsync(int id, int workLoad);
        Task<bool> CanTakeMoreLoadAsync(string examinerId, LoadType type);
        Task<ExaminerLoad> IncrementWorkloadAsync(string examinerId, LoadType type);
        Task<ExaminerLoad> DecrementWorkloadAsync(string examinerId, LoadType type);

    }
}
