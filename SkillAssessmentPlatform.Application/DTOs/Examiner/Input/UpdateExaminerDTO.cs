using SkillAssessmentPlatform.Application.DTOs.Auth.Inputs;
using System.ComponentModel.DataAnnotations;

namespace SkillAssessmentPlatform.Application.DTOs.Examiner.Input
{
    public class UpdateExaminerDTO : UpdateUserDTO
    {
        [Required]
        [MaxLength(100)]
        public string Bio { get; set; }
    }
}
