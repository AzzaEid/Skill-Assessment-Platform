using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Entities
{
    public class Enrollment
    {
        public int Id { get; set; }
        public string ApplicantId { get; set; }
        public int TrackId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public EnrollmentStatus Status { get; set; }


        public Applicant Applicant { get; set; }
        public Track Track { get; set; }
        public ICollection<LevelProgress> LevelProgresses { get; set; }

    }
}