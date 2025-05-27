namespace SkillAssessmentPlatform.Application.DTOs.ExaminerDashboard
{
    public class ExaminerDashboardSummaryDTO
    {
        public int PendingTaskSubmissions { get; set; }
        public int PendingInterviewRequests { get; set; }
        public int ScheduledInterviews { get; set; }
        public int PendingExamReviews { get; set; }
        public int PendingTaskCreations { get; set; }
        public int PendingExamCreations { get; set; }
        public int TotalPendingItems => PendingTaskSubmissions + PendingInterviewRequests +
                                       ScheduledInterviews + PendingExamReviews +
                                       PendingTaskCreations + PendingExamCreations;
    }

}
