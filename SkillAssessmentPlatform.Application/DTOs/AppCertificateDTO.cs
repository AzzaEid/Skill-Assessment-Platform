using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class AppCertificateDTO
    {
        public int Id { get; set; }
        public string ApplicantId { get; set; }
        public int LevelProgressId { get; set; }
        public DateTime IssueDate { get; set; }
        public string VerificationCode { get; set; }
    }
}
