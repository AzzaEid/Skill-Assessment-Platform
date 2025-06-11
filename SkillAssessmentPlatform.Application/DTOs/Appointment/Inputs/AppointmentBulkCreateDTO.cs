using System.ComponentModel.DataAnnotations;

namespace SkillAssessmentPlatform.Application.DTOs.Appointment.Inputs
{
    public class AppointmentBulkCreateDTO
    {
        [Required]
        public string ExaminerId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(15, 120)]
        public int SlotDurationMinutes { get; set; }

        [Required]
        [Range(8, 22)]
        public int StartHour { get; set; }

        [Required]
        [Range(8, 22)]
        public int EndHour { get; set; }
    }
}
