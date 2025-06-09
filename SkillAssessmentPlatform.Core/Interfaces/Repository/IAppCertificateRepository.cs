using SkillAssessmentPlatform.Core.Entities.Certificates_and_Notifications;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface IAppCertificateRepository
    {
        Task AddAsync(AppCertificate certificate);
        Task<AppCertificate?> GetByIdAsync(int id);
        Task<IEnumerable<AppCertificate>> GetByApplicantIdAsync(string applicantId);
        Task<AppCertificate?> GetByVerificationCodeAsync(string code);
        Task<AppCertificate?> GetByLevelIdAndApplicantId(int levelId, string applicantId);

    }
}
