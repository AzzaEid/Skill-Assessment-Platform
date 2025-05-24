using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class DetailedFeedbackDTO
    {
        public int Id { get; set; }
        public int FeedbackId { get; set; }
        public int EvaluationCriteriaId { get; set; }
        public string Comments { get; set; }
        public decimal Score { get; set; }
    }

}
