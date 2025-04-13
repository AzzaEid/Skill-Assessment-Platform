using SkillAssessmentPlatform.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class StageProgressDTO
    {
        public int Id { get; set; }
        public int EnrollmentId { get; set; }
        public int StageId { get; set; }
        public string StageName { get; set; }
        public StageType StageType { get; set; }
        public string Status { get; set; }
        public int Score { get; set; }
        public string ExaminerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int Attempts { get; set; }
    }

    public class UpdateStageStatusDTO
    {
        public string Status { get; set; }
        public int Score { get; set; }
    }

    public class AssignExaminerDTO
    {
        public string ExaminerId { get; set; }
    }
}
