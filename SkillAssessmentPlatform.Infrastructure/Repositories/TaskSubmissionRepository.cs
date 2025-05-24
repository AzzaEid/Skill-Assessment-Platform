using Microsoft.EntityFrameworkCore;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;


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
                // .ThenInclude(ta => ta.StageProgress)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<TaskSubmission>> GetByApplicantIdAsync(string applicantId)
        {
            return await _context.TaskSubmissions
                .Include(s => s.TaskApplicant)
                .Where(s => s.TaskApplicant.ApplicantId == applicantId)
                .ToListAsync();
        }
    }

}
