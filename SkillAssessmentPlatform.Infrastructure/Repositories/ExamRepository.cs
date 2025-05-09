using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;
using Task = System.Threading.Tasks.Task;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class ExamRepository : GenericRepository<Exam>, IExamRepository
    {

        public ExamRepository (AppDbContext context) : base(context)
        {
            //_context = context; 
        }
        public async System.Threading.Tasks.Task AddAsync(Exam exam)
        {
            await _context.Exams.AddAsync(exam);
        }

        public async Task<Exam> GetByStageIdAsync(int stageId)
        {
            return await _context.Exams.FirstOrDefaultAsync(e => e.StageId == stageId);
        }


        public async Task<Exam> GetByIdAsync(int id)
        {
            return await _context.Exams.FindAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam == null) return false;

            exam.IsActive = false;
            return true;
        }


    }
}
