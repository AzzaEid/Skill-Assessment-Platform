using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkillAssessmentPlatform.Application.DTOs.Enrollment;
using SkillAssessmentPlatform.Core.Common;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces;

namespace SkillAssessmentPlatform.Application.Services
{
    public class EnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EnrollmentService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<EnrollmentDTO>> GetAllEnrollmentsAsync(int page = 1, int pageSize = 10)
        {
            var enrollments = _unitOfWork.EnrollmentRepository.GetPagedQueryable(page, pageSize).Include(e => e.Track).ToList();
            var totalCount = await _unitOfWork.EnrollmentRepository.GetTotalCountAsync();

            return new PagedResponse<EnrollmentDTO>(
                _mapper.Map<List<EnrollmentDTO>>(enrollments),
                page,
                pageSize,
                totalCount
            );
        }

        public async Task<EnrollmentDTO> GetEnrollmentByIdAsync(int id)
        {
            var enrollment = await _unitOfWork.EnrollmentRepository.GetByIdAsync(id);
            return _mapper.Map<EnrollmentDTO>(enrollment);
        }

        public async Task<PagedResponse<EnrollmentDTO>> GetByApplicantIdAsync(string applicantId, int page = 1, int pageSize = 10)
        {
            var enrollments = await _unitOfWork.EnrollmentRepository.GetByApplicantIdAsync(applicantId, page, pageSize);
            var totalCount = await _unitOfWork.EnrollmentRepository.CountByApplicantIdAsync(applicantId);

            return new PagedResponse<EnrollmentDTO>(
                _mapper.Map<List<EnrollmentDTO>>(enrollments),
                page,
                pageSize,
                totalCount
            );
        }

        public async Task<EnrollmentDTO> EnrollApplicantInTrackAsync(string applicantId, EnrollmentCreateDTO enrollmentDto)
        {
            var applicant = await _unitOfWork.ApplicantRepository.GetByIdAsync(applicantId);
            if (applicant == null)
                throw new KeyNotFoundException($"Applicant with id {applicantId} not found");

            var track = await _unitOfWork.TrackRepository.GetByIdAsync(enrollmentDto.TrackId);
            if (track == null)
                throw new KeyNotFoundException($"Track with id {enrollmentDto.TrackId} not found");

            var existingEnrollment = await _unitOfWork.EnrollmentRepository.GetByApplicantAndTrackAsync(applicantId, enrollmentDto.TrackId);
            if (existingEnrollment != null)
                throw new BadRequestException("Applicant is already enrolled in this track");

            var enrollment = new Enrollment
            {
                ApplicantId = applicantId,
                TrackId = enrollmentDto.TrackId,
                EnrollmentDate = DateTime.Now,
                Status = EnrollmentStatus.Active,
            };

            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.EnrollmentRepository.AddAsync(enrollment);
                await _unitOfWork.SaveChangesAsync();

                var firstLevel = await _unitOfWork.LevelRepository.GetFirstLevelByTrackIdAsync(enrollmentDto.TrackId);
                if (firstLevel != null)
                {
                    var levelProgress = new LevelProgress
                    {
                        EnrollmentId = enrollment.Id,
                        LevelId = firstLevel.Id,
                        Status = ProgressStatus.InProgress,
                        StartDate = DateTime.Now
                    };

                    await _unitOfWork.LevelProgressRepository.AddAsync(levelProgress);
                    await _unitOfWork.SaveChangesAsync();

                    var firstStage = await _unitOfWork.StageRepository.GetFirstStageByLevelIdAsync(firstLevel.Id);
                    if (firstStage != null)
                    {
                        var freeExaminerId = await _unitOfWork.ExaminerRepository.GetAvailableExaminerAsync(enrollmentDto.TrackId, MapLoad(firstStage.Type));
                        if (freeExaminerId == null)
                            throw new InvalidOperationException("No available examiner found for this stage");

                        var stageProgress = new StageProgress
                        {
                            LevelProgressId = levelProgress.Id,
                            StageId = firstStage.Id,
                            Status = ProgressStatus.InProgress,
                            StartDate = DateTime.Now,
                            Attempts = 1,
                            ExaminerId = freeExaminerId.ToString()
                        };
                        await _unitOfWork.ExaminerLoadRepository.IncrementWorkloadAsync(freeExaminerId, MapLoad(firstStage.Type));
                        await _unitOfWork.StageProgressRepository.AddAsync(stageProgress);
                        await _unitOfWork.SaveChangesAsync();
                    }
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            return _mapper.Map<EnrollmentDTO>(enrollment);
        }
        private LoadType MapLoad(StageType stageType)
        {
            var type = LoadType.Task;
            if (stageType == StageType.Exam) { type = LoadType.Exam; }
            else if (stageType == StageType.Interview) { type = LoadType.Interview; }
            return type;
        }

        public async Task<EnrollmentDTO> UpdateEnrollmentStatusAsync(int enrollmentId, UpdateEnrollmentStatusDTO updateDto)
        {
            var enrollment = await _unitOfWork.EnrollmentRepository.UpdateStatusAsync(enrollmentId, updateDto.Status);
            return _mapper.Map<EnrollmentDTO>(enrollment);
        }
    }
}
