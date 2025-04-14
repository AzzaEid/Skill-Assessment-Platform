using SkillAssessmentPlatform.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs
{
    // Enrollment DTOs
    public class EnrollmentDTO
    {
        public int Id { get; set; }
        public string ApplicantId { get; set; }
        public int TrackId { get; set; }
        public string TrackName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public EnrollmentStatus Status { get; set; }
    }

    public class EnrollmentCreateDTO
    {
        public int TrackId { get; set; }
    }

    public class UpdateEnrollmentStatusDTO
    {
        public EnrollmentStatus Status { get; set; }
    }
}
