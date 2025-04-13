using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class ExaminerLoadDTO
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public int MaxWorkLoad { get; set; }
        public int CurrWorkLoad { get; set; }
    }
    public class UpdateWorkLoadDTO
    {
        public int WorkLoad { get; set; }
    }

    public class CreateExaminerLoadDTO
    {
        public string ExaminerID { get; set; }
        public string Type { get; set; }
        public int MaxWorkLoad { get; set; }
        public int CurrWorkLoad { get; set; } = 0;
    }
}
