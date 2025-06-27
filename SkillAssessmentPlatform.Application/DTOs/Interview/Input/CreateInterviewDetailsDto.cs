using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class CreateInterviewDetailsDto
    {
        public int MaxDaysToBook { get; set; }
        public int DurationMinutes { get; set; }
        public string Instructions { get; set; }
    }

}
