namespace SkillAssessmentPlatform.Application.DTOs.Feedback.Output
{
    public class FeedbackWithInfoDTO
    {

        public int Id { get; set; }
        public string Comments { get; set; }
        public decimal TotalScore { get; set; }
        public DateTime FeedbackDate { get; set; }
        public List<DetailedFeedbackDTO> DetailedFeedbacks { get; set; }

        public TaskSubmissionInfoDto TaskSubmissionInfo { get; set; }
        public InterviewInfoDto InterviewInfo { get; set; }
        public ExamInfoExaminerDto ExamInfo { get; set; }
    }

    public class TaskSubmissionInfoDto
    {
        public string ApplicantName { get; set; }
        public string TaskTitle { get; set; }
        public string SubmissionUrl { get; set; }
        public DateTime SubmissionDate { get; set; }
    }
    public class InterviewInfoDto
    {
        public string ApplicantName { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public string InterviewInstructions { get; set; }
    }
    public class ExamInfoExaminerDto
    {
        public string ApplicantName { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string ExamDifficulty { get; set; }
    }


}

