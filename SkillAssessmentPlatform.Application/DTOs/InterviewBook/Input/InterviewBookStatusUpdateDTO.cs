using SkillAssessmentPlatform.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace SkillAssessmentPlatform.Application.DTOs.InterviewBook.Input
{
    public class InterviewBookStatusUpdateDTO
    {
        [Required]
        [RegularExpression("^(Scheduled|Completed|Canceled)$",
            ErrorMessage = "Status must be one of: Scheduled, Completed, Canceled")]
        public InterviewStatus Status { get; set; }
    }
}
