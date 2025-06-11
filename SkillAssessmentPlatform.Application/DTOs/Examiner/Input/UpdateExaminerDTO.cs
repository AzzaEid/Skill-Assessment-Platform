using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillAssessmentPlatform.Application.DTOs.Auth.Inputs;

namespace SkillAssessmentPlatform.Application.DTOs.Examiner.Input
{
    public class UpdateExaminerDTO : UpdateUserDTO
    {
        [Required]
        [MaxLength(100)]
        public string Specialization { get; set; }
    }
}
