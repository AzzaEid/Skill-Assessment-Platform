using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Certificates_and_Notifications;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface IApplicantRepository : IGenericRepository<Applicant>
    {
        Task<Applicant> UpdateStatusAsync(string id, ApplicantStatus status);
        Task<IEnumerable<Enrollment>> GetEnrollmentsAsync(string applicantId);
        Task<IEnumerable<AppCertificate>> GetCertificatesAsync(string applicantId);
        Task<Enrollment> EnrollInTrackAsync(string applicantId, int trackId);
        Task<IEnumerable<LevelProgress>> GetProgressAsync(string applicantId);
    }
}
