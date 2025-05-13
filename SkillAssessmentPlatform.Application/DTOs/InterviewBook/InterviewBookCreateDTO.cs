using System.ComponentModel.DataAnnotations;

namespace SkillAssessmentPlatform.Application.DTOs.InterviewBook
{

    public class InterviewBookCreateDTO
    {
        [Required]
        public int InterviewId { get; set; }

        [Required]
        public int AppointmentId { get; set; }

    }

}
