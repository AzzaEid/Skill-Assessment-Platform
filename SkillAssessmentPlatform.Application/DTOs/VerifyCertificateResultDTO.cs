using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class VerifyCertificateResultDTO
    {
        public string ApplicantFullName { get; set; }
        public string TrackName { get; set; }
        public string LevelName { get; set; }
        public DateTime IssueDate { get; set; }
        public string VerificationCode { get; set; }
    }

}
