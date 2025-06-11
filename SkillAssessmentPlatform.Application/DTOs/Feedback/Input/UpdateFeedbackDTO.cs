using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class UpdateFeedbackDTO
    {
        public string Comments { get; set; }
        public decimal TotalScore { get; set; }
    }

}
