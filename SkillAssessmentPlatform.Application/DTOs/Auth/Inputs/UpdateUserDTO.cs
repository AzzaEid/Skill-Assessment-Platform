using SkillAssessmentPlatform.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace SkillAssessmentPlatform.Application.DTOs.Auth.Inputs
{
    public class UpdateUserDTO
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public Gender? Gender { get; set; }
    }
}
