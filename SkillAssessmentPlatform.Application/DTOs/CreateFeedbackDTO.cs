using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class CreateFeedbackDTO
    {
        public string ExaminerId { get; set; }
        public string Comments { get; set; }
        public decimal TotalScore { get; set; }
        public int? TaskSubmissionId { get; set; }
        public int? ExamRequestId { get; set; }
        public int? InterviewBookId { get; set; }

        public List<CreateDetailedFeedbackDTO> DetailedFeedbacks { get; set; }
    }

}
