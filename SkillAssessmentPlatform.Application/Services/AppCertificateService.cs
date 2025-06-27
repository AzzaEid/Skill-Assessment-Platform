using SkillAssessmentPlatform.Application.DTOs.Certificate.Input;
using SkillAssessmentPlatform.Application.DTOs.Certificate.Output;
using SkillAssessmentPlatform.Core.Entities.Certificates_and_Notifications;
using SkillAssessmentPlatform.Core.Interfaces;

namespace SkillAssessmentPlatform.Application.Services
{
    public class AppCertificateService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppCertificateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AppCertificateDTO> CreateAsync(CreateAppCertificateDTO dto)
        {
            var certificate = new AppCertificate
            {
                ApplicantId = dto.ApplicantId,
                LeveProgressId = dto.LevelProgressId,
                IssueDate = DateTime.UtcNow,
                VerificationCode = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper()
            };

            await _unitOfWork.AppCertificateRepository.AddAsync(certificate);
            await _unitOfWork.SaveChangesAsync();

            return new AppCertificateDTO
            {
                Id = certificate.Id,
                ApplicantId = certificate.ApplicantId,
                LevelProgressId = certificate.LeveProgressId,
                IssueDate = certificate.IssueDate,
                VerificationCode = certificate.VerificationCode
            };
        }

        public async Task<IEnumerable<AppCertificateDTO>> GetByApplicantIdAsync(string applicantId)
        {
            var result = await _unitOfWork.AppCertificateRepository.GetByApplicantIdAsync(applicantId);

            return result.Select(c => new AppCertificateDTO
            {
                Id = c.Id,
                ApplicantId = c.ApplicantId,
                LevelProgressId = c.LeveProgressId,
                IssueDate = c.IssueDate,
                VerificationCode = c.VerificationCode
            });
        }

        public async Task<CertificateViewModel?> VerifyByCodeAsync(string code)
        {
            var certificate = await _unitOfWork.AppCertificateRepository.GetByVerificationCodeAsync(code);
            if (certificate == null)
                return null;

            return new CertificateViewModel
            {
                ApplicantName = certificate.Applicant.FullName,
                TrackName = certificate.LevelProgress.Level.Track.Name,
                LevelName = certificate.LevelProgress.Level.Name,
                IssueDate = certificate.IssueDate,
                VerificationCode = certificate.VerificationCode
            };
        }
        public async Task<AppCertificateDTO?> GetByIdAsync(int id)
        {
            var cert = await _unitOfWork.AppCertificateRepository.GetByIdAsync(id);
            if (cert == null) return null;

            return new AppCertificateDTO
            {
                Id = cert.Id,
                ApplicantId = cert.ApplicantId,
                LevelProgressId = cert.LeveProgressId,
                IssueDate = cert.IssueDate,
                VerificationCode = cert.VerificationCode
            };
        }
        public async Task<string> GetByLevelIdAndApplicantId(int levelId, string applicantId)
        {
            var certificate = await _unitOfWork.AppCertificateRepository.GetByLevelIdAndApplicantId(levelId, applicantId);
            if (certificate == null) return null;

            return certificate.VerificationCode;
        }

    }
}

