namespace SkillAssessmentPlatform.Application.DTOs.StageProgress
{
    public enum StageActionStatus
    {
        // For Exam stages
        ReadyToRequest,        // No exam request exists
        RequestPending,        // Exam request is pending
        RequestApproved,       // Exam request approved, ready to take exam
        RequestRejected,       // Request rejected, can request new attempt
        ExamCompleted,         // Exam taken, waiting for results
        FeedbackAvailable,     // Results available, need to fetch feedback

        // For Interview stages
        ReadyToBook,           // No interview booking exists
        BookingPending,        // Interview booking request pending
        BookingScheduled,      // Interview scheduled
        BookingCanceled,       // Booking canceled, can book again
        InterviewCompleted,    // Interview completed, waiting for results

        // For Task stages
        TaskNotAssigned,
        TaskAssigned,          // Task assigned, no submission yet
        TaskSubmitted,         // Task submitted, under review
        TaskAccepted,          // Task accepted
        TaskRejected,          // Task rejected, may need resubmission

        // Common statuses
        Reviewed,              // Feedback is available
        Failed,                // Stage failed
        Completed              // Stage completed successfully
    }
}
