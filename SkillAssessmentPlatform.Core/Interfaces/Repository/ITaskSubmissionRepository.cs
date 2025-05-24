using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface ITaskSubmissionRepository
    {
        Task AddAsync(TaskSubmission submission);
        Task<TaskSubmission?> GetByIdAsync(int id);
        Task<IEnumerable<TaskSubmission>> GetByApplicantIdAsync(string applicantId);
        Task<TaskSubmission?> GetByStageProgressIdAsync(int stageProgressId);

    }

}
