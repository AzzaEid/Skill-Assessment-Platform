using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class TaskSubmissionDTO
    {
        public int Id { get; set; }
        public int TaskApplicantId { get; set; }
        public string SubmissionUrl { get; set; }
        public DateTime SubmissionDate { get; set; }
    }

}
