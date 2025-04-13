using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface IEnrollmentRepository : IGenericRepository<Enrollment>
    {
        Task<IEnumerable<Enrollment>> GetByApplicantIdAsync(string applicantId, int page = 1, int pageSize = 10);
        Task<int> CountByApplicantIdAsync(string applicantId);
        Task<Enrollment> GetByApplicantAndTrackAsync(string applicantId, int trackId);
        Task<Enrollment> UpdateStatusAsync(int enrollmentId, EnrollmentStatus status);
        Task<Enrollment> GetLatestActiveEnrollmentAsync(string applicantId);
    }
}
