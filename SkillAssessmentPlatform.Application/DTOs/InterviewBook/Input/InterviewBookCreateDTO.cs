using System.ComponentModel.DataAnnotations;

namespace SkillAssessmentPlatform.Application.DTOs.InterviewBook.Input
{

    public class InterviewBookCreateDTO
    {
        [Required]
        public int InterviewId { get; set; }

        [Required]
        public int AppointmentId { get; set; }

    }

}
