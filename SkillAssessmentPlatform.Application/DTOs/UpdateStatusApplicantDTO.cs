using SkillAssessmentPlatform.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class UpdateStatusDTO
    {
        [Required]
        public ApplicantStatus Status { get; set; }
    }
}
