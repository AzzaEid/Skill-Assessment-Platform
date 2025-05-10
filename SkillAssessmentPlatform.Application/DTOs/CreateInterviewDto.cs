using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class CreateInterviewDto
    {

        public int StageId { get; set; }
        public int MaxDaysToBook { get; set; }
        public int DurationMinutes { get; set; }
        public string Instructions { get; set; }

        public InterviewStatus Status { get; set; } = InterviewStatus.Scheduled;

    }
}
