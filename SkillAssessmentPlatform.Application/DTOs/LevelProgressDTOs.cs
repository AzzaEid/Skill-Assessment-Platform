using SkillAssessmentPlatform.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class LevelProgressDTO
    {
        public int Id { get; set; }
        public int EnrollmentId { get; set; }
        public int LevelId { get; set; }
        public string LevelName { get; set; }
        public ProgressStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
    }

    public class UpdateLevelStatusDTO
    {
        public ProgressStatus Status { get; set; }
    }
}
