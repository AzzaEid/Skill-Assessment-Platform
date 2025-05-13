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
        //public StageType Type { get; set; }

        public int MaxWorkLoad { get; set; }
    }

    public class CreateExaminerLoadDTO
    {
        public string ExaminerID { get; set; }
        public string Type { get; set; }
        public int MaxWorkLoad { get; set; }
        public int CurrWorkLoad { get; set; } = 0;
    }
}
