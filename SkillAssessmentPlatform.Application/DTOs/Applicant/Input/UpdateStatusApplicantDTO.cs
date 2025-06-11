using SkillAssessmentPlatform.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace SkillAssessmentPlatform.Application.DTOs.Applicant.Input
{
    public class UpdateStatusDTO
    {
        [Required]
        public ApplicantStatus Status { get; set; }
    }
}
