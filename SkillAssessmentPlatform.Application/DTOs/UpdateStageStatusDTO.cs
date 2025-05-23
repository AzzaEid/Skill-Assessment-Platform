using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class UpdateStageStatusDTO
    {
        public ProgressStatus Status { get; set; }
        public int Score { get; set; }
    }

}
