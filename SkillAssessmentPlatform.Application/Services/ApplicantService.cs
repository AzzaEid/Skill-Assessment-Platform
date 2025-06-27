using AutoMapper;
using SkillAssessmentPlatform.Application.DTOs.Applicant.Input;
using SkillAssessmentPlatform.Application.DTOs.Applicant.Outputs;
using SkillAssessmentPlatform.Core.Common;
using SkillAssessmentPlatform.Core.Interfaces;

namespace SkillAssessmentPlatform.Application.Services
{
    public class ApplicantService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public ApplicantService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<ApplicantDTO>> GetAllApplicantsAsync(int page = 1, int pageSize = 10)
        {
            //var applicants = await _applicantRepository.GetPagedQueryable(page, pageSize);
            //var totalCount = await _applicantRepository.CountAsync();

            var applicants = _unitOfWork.ApplicantRepository.GetPagedQueryable(page, pageSize);
            var totalCount = await _unitOfWork.ApplicantRepository.GetTotalCountAsync();
            return new PagedResponse<ApplicantDTO>(
                _mapper.Map<List<ApplicantDTO>>(applicants),
                page,
                pageSize,
                totalCount
            );
        }

        public async Task<ApplicantDTO> GetApplicantByIdAsync(string id)
        {
            var applicant = await _unitOfWork.ApplicantRepository.GetByIdAsync(id);
            return _mapper.Map<ApplicantDTO>(applicant);
        }

        public async Task<ApplicantDTO> UpdateApplicantStatusAsync(string id, UpdateStatusDTO updateStatusDto)
        {
            var applicant = await _unitOfWork.ApplicantRepository.GetByIdAsync(id);
            applicant.Status = updateStatusDto.Status;

            var updatedApplicant = await _unitOfWork.ApplicantRepository.UpdateAsync(applicant);
            return _mapper.Map<ApplicantDTO>(updatedApplicant);
        }
        /*
        public async AppTask<PagedResponse<EnrollmentDTO>> GetApplicantEnrollmentsAsync(string applicantId, int page = 1, int pageSize = 10)
        {
            var enrollments = await _enrollmentRepository.GetByApplicantIdAsync(applicantId, page, pageSize);
            var totalCount = await _enrollmentRepository.CountByApplicantIdAsync(applicantId);

            return new PagedResponse<EnrollmentDTO>(
                _mapper.Map<List<EnrollmentDTO>>(enrollments),
                page,
                pageSize,
                totalCount
            );
        }

        public async AppTask<PagedResponse<CertificateDTO>> GetApplicantCertificatesAsync(string applicantId, int page = 1, int pageSize = 10)
        {
            var certificates = await _certificateRepository.GetByApplicantIdAsync(applicantId, page, pageSize);
            var totalCount = await _certificateRepository.CountByApplicantIdAsync(applicantId);

            return new PagedResponse<CertificateDTO>(
                _mapper.Map<List<CertificateDTO>>(certificates),
                page,
                pageSize,
                totalCount
            );
        }
        
        public async AppTask<EnrollmentDTO> EnrollApplicantInTrackAsync(string applicantId, EnrollmentCreateDTO enrollmentDto)
        {
            var track = await _trackRepository.GetByIdAsync(enrollmentDto.TrackId);
            var applicant = await _applicantRepository.GetByIdAsync(applicantId);

            var enrollment = new Enrollment
            {
                ApplicantId = applicantId,
                TrackId = enrollmentDto.TrackId,
                EnrollmentDate = DateTime.UtcNow,
                Status = EnrollmentStatus.Pending
            };

            var createdEnrollment = await _enrollmentRepository.CreateAsync(enrollment);
            return _mapper.Map<EnrollmentDTO>(createdEnrollment);
        }

        public async AppTask<PagedResponse<ProgressDTO>> GetApplicantProgressAsync(string applicantId, int page = 1, int pageSize = 10)
        {
            var progressRecords = await _enrollmentRepository.GetProgressByApplicantIdAsync(applicantId, page, pageSize);
            var totalCount = await _enrollmentRepository.CountProgressByApplicantIdAsync(applicantId);

            return new PagedResponse<ProgressDTO>(
                _mapper.Map<List<ProgressDTO>>(progressRecords),
                page,
                pageSize,
                totalCount
            );
        }
        */
    }

}
