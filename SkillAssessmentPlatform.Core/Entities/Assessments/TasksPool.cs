using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews
{
    public class TasksPool
    {
        public int Id { get; set; }
        public int StageId { get; set; }
        public int DaysToSubmit { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        // Navigation properties
        public Stage Stage { get; set; }
        public ICollection<AppTask> Tasks { get; set; } = new List<AppTask>();
    }
}
