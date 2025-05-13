using System.ComponentModel.DataAnnotations;

namespace SkillAssessmentPlatform.Application.DTOs.Appointment
{

    public class AppointmentSingleCreateDTO
    {
        [Required]
        public string ExaminerId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
    }
}
