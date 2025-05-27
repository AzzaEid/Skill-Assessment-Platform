using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Core.Entities.Users
{
    public class ExaminerLoad
    {
        public int ID { get; set; }

        public string ExaminerID { get; set; }
        public LoadType Type { get; set; }

        public int MaxWorkLoad { get; set; }

        public int CurrWorkLoad { get; set; } = 0;


        // Navigation properties
        public Examiner Examiner { get; set; }
    }
}
