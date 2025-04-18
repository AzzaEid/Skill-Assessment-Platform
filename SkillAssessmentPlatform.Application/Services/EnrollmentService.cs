using AutoMapper;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Common;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var enrollments = await _unitOfWork.EnrollmentRepository.GetPagedAsync(page, pageSize);
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
            // Check if applicant exists
            var applicant = await _unitOfWork.ApplicantRepository.GetByIdAsync(applicantId);

            // Check if track exists
            var track = await _unitOfWork.TrackRepository.GetByIdAsync(enrollmentDto.TrackId);
            if (track == null)
                throw new KeyNotFoundException($"Track with id {enrollmentDto.TrackId} not found");

            // Check if enrollment already exists
            var existingEnrollment = await _unitOfWork.EnrollmentRepository.GetByApplicantAndTrackAsync(applicantId, enrollmentDto.TrackId);
            if (existingEnrollment != null)
                throw new BadRequestException("Applicant is already enrolled in this track");

            // Create enrollment
            var enrollment = new Enrollment
            {
                ApplicantId = applicantId,
                TrackId = enrollmentDto.TrackId,
                EnrollmentDate = DateTime.Now,
                Status = EnrollmentStatus.Active,
            };
            //>>> add transaction
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                await _unitOfWork.EnrollmentRepository.AddAsync(enrollment);
                await _unitOfWork.SaveChangesAsync();

                // Get the first level of the track
                var firstLevel = await _unitOfWork.LevelRepository.GetFirstLevelByTrackIdAsync(enrollmentDto.TrackId);
                if (firstLevel != null)
                {
                    // Create level progress
                    var levelProgress = new LevelProgress
                    {
                        EnrollmentId = enrollment.Id,
                        LevelId = firstLevel.Id,
                        Status = ProgressStatus.InProgress,
                        StartDate = DateTime.Now
                    };

                    await _unitOfWork.LevelProgressRepository.AddAsync(levelProgress);


                    // Get the first stage of the level
                    var firstStage = await _unitOfWork.StageRepository.GetFirstStageByLevelIdAsync(firstLevel.Id);

                    // Search for supervisor 
                    var freeExaminerId = await _unitOfWork.ExaminerRepository.GetAvailableExaminerAsync(firstStage.Type);
                    if (freeExaminerId == null)
                        throw new InvalidOperationException("No available examiner found for this stage");

                    if (firstStage != null)
                    {
                        // Create stage progress
                        var stageProgress = new StageProgress
                        {
                            LevelProgressId = levelProgress.Id,
                            StageId = firstStage.Id,
                            Status = ProgressStatus.InProgress ,
                            StartDate = DateTime.Now,
                            Attempts = 1,
                            ExaminerId = freeExaminerId.ToString()
                        };

                        await _unitOfWork.StageProgressRepository.AddAsync(stageProgress);
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }

            return _mapper.Map<EnrollmentDTO>(enrollment);
        }

        public async Task<EnrollmentDTO> UpdateEnrollmentStatusAsync(int enrollmentId, UpdateEnrollmentStatusDTO updateDto)
        {
            var enrollment = await _unitOfWork.EnrollmentRepository.UpdateStatusAsync(enrollmentId, updateDto.Status);
            return _mapper.Map<EnrollmentDTO>(enrollment);
        }
    }
}
