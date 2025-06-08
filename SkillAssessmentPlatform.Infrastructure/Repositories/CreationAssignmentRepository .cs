using Microsoft.EntityFrameworkCore;
using SkillAssessmentPlatform.Core.Entities.Management;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class CreationAssignmentRepository : GenericRepository<CreationAssignment>, ICreationAssignmentRepository
    {
        public CreationAssignmentRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<CreationAssignment>> GetByExaminerIdAsync(string examinerId)
        {
            return await _context.CreationAssignments
                .Where(ca => ca.ExaminerId == examinerId)
                .Include(ca => ca.Stage)
                    .ThenInclude(s => s.Level)
                    .ThenInclude(l => l.Track)
                .Include(ca => ca.AssignedBySenior)
                .OrderByDescending(ca => ca.AssignedDate)
            .ToListAsync();
        }

        public async Task<IEnumerable<CreationAssignment>> GetByExaminerIdAndTypeAsync(string examinerId, CreationType type)
        {
            return await _context.CreationAssignments
                .Where(ca => ca.ExaminerId == examinerId && ca.Type == type)
                .Include(ca => ca.Stage)
                    .ThenInclude(s => s.Level)
                    .ThenInclude(l => l.Track)
                .OrderByDescending(ca => ca.AssignedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<CreationAssignment>> GetPendingByExaminerIdAsync(string examinerId)
        {
            return await _context.CreationAssignments
                .Where(ca => ca.ExaminerId == examinerId &&
                            (ca.Status == AssignmentStatus.Assigned))
                .Include(ca => ca.Stage)
                    .ThenInclude(s => s.Level)
                    .ThenInclude(l => l.Track)
                .OrderBy(ca => ca.DueDate)
            .ToListAsync();
        }
        public async Task<IEnumerable<CreationAssignment>> GetPendingTasksByExaminerIdAsync(string examinerId)
        {
            var assignments = await _context.CreationAssignments
                .Where(ca => ca.ExaminerId == examinerId && ca.Type == CreationType.Task &&
                  (ca.DueDate < DateTime.UtcNow || ca.Status == AssignmentStatus.Overdue || ca.Status == AssignmentStatus.Assigned))
                .Include(ca => ca.Examiner)
                .Include(ca => ca.Stage)
                    .ThenInclude(s => s.TasksPool)
                .Include(ca => ca.Stage)
                    .ThenInclude(s => s.Exam)
                .OrderByDescending(ca => ca.AssignedDate)
                .ToListAsync();

            foreach (var assignment in assignments)
            {
                if (assignment.DueDate < DateTime.UtcNow)
                {
                    assignment.Status = AssignmentStatus.Overdue;
                }
            }
            return assignments;
        }
        public async Task<IEnumerable<CreationAssignment>> GetPendingExamsByExaminerIdAsync(string examinerId)
        {
            var assignments = await _context.CreationAssignments
                .Where(ca => ca.ExaminerId == examinerId && ca.Type == CreationType.Exam &&
                            (ca.DueDate < DateTime.UtcNow || ca.Status == AssignmentStatus.Overdue || ca.Status == AssignmentStatus.Assigned))
                .Include(ca => ca.Stage)
                    .ThenInclude(s => s.Level)
                    .ThenInclude(l => l.Track)
                .OrderBy(ca => ca.DueDate)
            .ToListAsync();
            foreach (var assignment in assignments)
            {
                if (assignment.DueDate < DateTime.UtcNow)
                {
                    assignment.Status = AssignmentStatus.Overdue;
                }
            }
            return assignments;
        }
        public async Task<IEnumerable<CreationAssignment>> GetPendingBySeniorAsync(string seniorId)
        {
            var now = DateTime.Now;
            var assignments = await _context.CreationAssignments
                .Where(ca => ca.AssignedBySeniorId == seniorId &&
                (ca.DueDate < now || ca.Status == AssignmentStatus.Overdue || ca.Status == AssignmentStatus.Assigned))
                .Include(ca => ca.Examiner)
                .Include(ca => ca.Stage)
                    .ThenInclude(s => s.TasksPool)
                .Include(ca => ca.Stage)
                    .ThenInclude(s => s.Exam)
                .OrderByDescending(ca => ca.AssignedDate)
                .ToListAsync();

            foreach (var assignment in assignments)
            {
                if (assignment.DueDate < now)
                {
                    assignment.Status = AssignmentStatus.Overdue;
                }
            }

            return assignments;
        }
        public async Task<IEnumerable<CreationAssignment>> GetOverdueAssignmentsAsync()
        {
            var now = DateTime.Now;
            var assignments = await _context.CreationAssignments
                .Where(ca => ca.DueDate < now || ca.Status == AssignmentStatus.Overdue || ca.Status == AssignmentStatus.Assigned)
                .Include(ca => ca.Examiner)
                .Include(ca => ca.Stage)
                    .ThenInclude(s => s.TasksPool)
                .Include(ca => ca.Stage)
                    .ThenInclude(s => s.Exam)
                .OrderByDescending(ca => ca.AssignedDate)
                .ToListAsync();

            foreach (var assignment in assignments)
            {
                if (assignment.DueDate < now)
                {
                    assignment.Status = AssignmentStatus.Overdue;
                }
            }

            return assignments;
        }
        public override async Task<IEnumerable<CreationAssignment>> GetAllAsync()
        {
            return await _context.CreationAssignments
                .Where(ca => ca.Status == AssignmentStatus.Assigned ||
                 ca.Status == AssignmentStatus.Overdue)
                .Include(ca => ca.Stage)
                    .ThenInclude(s => s.Level)
                    .ThenInclude(l => l.Track)
                .Include(ca => ca.AssignedBySenior)
                .OrderByDescending(ca => ca.AssignedDate)
            .ToListAsync();
        }

        public async Task<CreationAssignment> UpdateStatusAsync(int assignmentId, AssignmentStatus status)
        {
            var assignment = await _context.CreationAssignments.FindAsync(assignmentId);
            if (assignment == null)
                throw new KeyNotFoundException($"Assignment with id {assignmentId} not found");

            assignment.Status = status;

            _context.CreationAssignments.Update(assignment);
            await _context.SaveChangesAsync();
            return assignment;
        }
        public async Task<CreationAssignment>? GetByExaminerAndStageAsync(string examinerId, int stageId)
        {
            return await _context.CreationAssignments
               .Where(ca => ca.StageId == stageId &&
                           ca.ExaminerId == examinerId &&
                           ca.Type == CreationType.Task &&
                           ca.Status != AssignmentStatus.Completed)
               .Include(a => a.Examiner)
               .Include(a => a.Stage)
               .FirstOrDefaultAsync();

        }

    }


}

