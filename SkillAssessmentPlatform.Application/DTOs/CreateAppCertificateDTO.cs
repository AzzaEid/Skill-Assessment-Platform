using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class CreateAppCertificateDTO
    {
        public string ApplicantId { get; set; }
        public int LevelProgressId { get; set; }
    }
}
