using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews
{
    public class AppTask
    {
        public int Id { get; set; }
        public int TaskPoolId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public string Difficulty { get; set; } //easy, med, hard
        // Navigation properties
        public TasksPool TasksPool { get; set; }
        public ICollection<TaskApplicant> TaskApplicants { get; set; } = new List<TaskApplicant>();
    }
}
