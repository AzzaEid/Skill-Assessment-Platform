using SkillAssessmentPlatform.Core.Entities.Management;
using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface ICreationAssignmentRepository : IGenericRepository<CreationAssignment>
    {
        Task<IEnumerable<CreationAssignment>> GetByExaminerIdAsync(string examinerId);
        Task<IEnumerable<CreationAssignment>> GetByExaminerIdAndTypeAsync(string examinerId, CreationType type);
        Task<IEnumerable<CreationAssignment>> GetPendingByExaminerIdAsync(string examinerId);
        Task<IEnumerable<CreationAssignment>> GetPendingTasksByExaminerIdAsync(string examinerId);
        Task<IEnumerable<CreationAssignment>> GetPendingExamsByExaminerIdAsync(string examinerId);

        Task<IEnumerable<CreationAssignment>> GetOverdueAssignmentsAsync();
        Task<CreationAssignment> UpdateStatusAsync(int assignmentId, AssignmentStatus status);
        Task<IEnumerable<CreationAssignment>> GetPendingBySeniorAsync(string seniorId);
        Task<CreationAssignment>? GetByExaminerAndStageAsync(string examinerId, int stageId);

    }
}
