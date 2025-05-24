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
    public class TaskSubmissionRepository : ITaskSubmissionRepository
    {
        private readonly AppDbContext _context;

        public TaskSubmissionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TaskSubmission submission)
        {
            await _context.TaskSubmissions.AddAsync(submission);
        }

        public async Task<TaskSubmission?> GetByIdAsync(int id)
        {
            return await _context.TaskSubmissions
                .Include(s => s.TaskApplicant)
                .ThenInclude(ta => ta.StageProgress)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<TaskSubmission>> GetByApplicantIdAsync(string applicantId)
        {
            return await _context.TaskSubmissions
                .Include(s => s.TaskApplicant)
                .Where(s => s.TaskApplicant.StageProgress.ApplicantId == applicantId)
                .ToListAsync();
        }


        public async Task<TaskSubmission?> GetByStageProgressIdAsync(int stageProgressId)
        {
            return await _context.TaskSubmissions
                .Include(ts => ts.TaskApplicant)
                    .ThenInclude(ta => ta.Task)
                        .ThenInclude(t => t.TasksPool)
                .FirstOrDefaultAsync(ts => ts.TaskApplicant.StageProgressId == stageProgressId);
        }


    }

}
