using Microsoft.EntityFrameworkCore;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;

public class TaskApplicantRepository : ITaskApplicantRepository
{
    private readonly AppDbContext _context;

    public TaskApplicantRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TaskApplicant taskApplicant)
    {
        await _context.TaskApplicants.AddAsync(taskApplicant);
    }

    public async Task<TaskApplicant?> GetByIdAsync(int id)
    {
        return await _context.TaskApplicants
            .Include(t => t.Task)
            .Include(t => t.Applicant) // موجود عندك Navigation Property ل Applicant
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<TaskApplicant>> GetByApplicantIdAsync(string applicantId)
    {
        return await _context.TaskApplicants
            .Include(t => t.Task)
            .Include(t => t.Applicant)
            .Where(t => t.ApplicantId == applicantId)
            .ToListAsync();
    }
}
