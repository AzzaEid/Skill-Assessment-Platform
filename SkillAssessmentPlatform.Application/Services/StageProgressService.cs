using AutoMapper;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.Services
{
    public class StageProgressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StageProgressService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StageProgressDTO> GetByIdAsync(int id)
        {
            var stageProgress = await _unitOfWork.StageProgressRepository.GetByIdAsync(id);
            return _mapper.Map<StageProgressDTO>(stageProgress);
        }

        public async Task<IEnumerable<StageProgressDTO>> GetByEnrollmentIdAsync(int enrollmentId)
        {
            var stageProgresses = await _unitOfWork.StageProgressRepository.GetByEnrollmentIdAsync(enrollmentId);
            return _mapper.Map<IEnumerable<StageProgressDTO>>(stageProgresses);
        }

        public async Task<StageProgressDTO> GetCurrentStageProgressAsync(int enrollmentId)
        {
            var stageProgress = await _unitOfWork.StageProgressRepository.GetCurrentStageProgressAsync(enrollmentId);
            return _mapper.Map<StageProgressDTO>(stageProgress);
        }

        public async Task<StageProgressDTO> UpdateStatusAsync(int stageProgressId, UpdateStageStatusDTO updateDto)
        {
            var stageProgress = await _unitOfWork.StageProgressRepository.UpdateStatusAsync(
                stageProgressId,
                updateDto.Status,
                updateDto.Score);

            var stage = await _unitOfWork.StageRepository.GetByIdAsync(stageProgress.StageId);

            // If stage completed successfully and score meets passing criteria
            if (updateDto.Status == "Successful" && updateDto.Score >= stage.PassingScore)
            {
                // Create progress for next stage
                var nextStageProgress = await _unitOfWork.StageProgressRepository.CreateNextStageProgressAsync(
                    stageProgress.EnrollmentId,
                    stageProgress.StageId);

                // If no next stage, level is completed
                if (nextStageProgress == null)
                {
                    // Get current level progress
                    var levelProgress = await _unitOfWork.LevelProgressRepository.GetCurrentLevelProgressAsync(stageProgress.EnrollmentId);

                    // Update level status to successful
                    await _unitOfWork.LevelProgressRepository.UpdateStatusAsync(levelProgress.Id, "Successful");
                }
            }
            // If stage failed but retries are possible
            else if (updateDto.Status == "Failed")
            {
                // Can create a new attempt if needed
                // This would depend on business rules
            }

            return _mapper.Map<StageProgressDTO>(stageProgress);
        }
        public async Task<StageProgressDTO> AssignExaminerAsync(int stageProgressId, AssignExaminerDTO assignDto)
        {
            // Get stage to check its type
            var stageProgress = await _unitOfWork.StageProgressRepository.GetByIdAsync(stageProgressId);
            if (stageProgress == null)
                throw new KeyNotFoundException($"StageProgress with id {stageProgressId} not found");

            var stage = await _unitOfWork.StageRepository.GetByIdAsync(stageProgress.StageId);
            if (stage == null)
                throw new KeyNotFoundException($"Stage with id {stageProgress.StageId} not found");

            // Check if examiner exists
            var examiner = await _unitOfWork.ExaminerRepository.GetByIdAsync(assignDto.ExaminerId);
            if (examiner == null)
                throw new KeyNotFoundException($"Examiner with id {assignDto.ExaminerId} not found");

            // Check if examiner can take more load
            var canTakeMore = await _unitOfWork.ExaminerLoadRepository.CanTakeMoreLoadAsync(
                assignDto.ExaminerId,
                stage.Type);

            if (!canTakeMore)
                throw new BadRequestException("Examiner has reached maximum workload for this stage type");

            // Assign examiner
            var updatedStageProgress = await _unitOfWork.StageProgressRepository.AssignExaminerAsync(
                stageProgressId,
                assignDto.ExaminerId);

            // Update examiner's current workload
            await _unitOfWork.ExaminerLoadRepository.IncrementWorkloadAsync(
                assignDto.ExaminerId,
                stage.Type);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<StageProgressDTO>(updatedStageProgress);
        }

        public async Task<StageProgressDTO> GetCurrentStageForApplicantAsync(string applicantId)
        {
            // Get the latest active enrollment for the applicant
            var enrollment = await _unitOfWork.EnrollmentRepository
                .GetLatestActiveEnrollmentAsync(applicantId);

            if (enrollment == null)
                throw new BadRequestException("No active enrollment found for the applicant");

            var currentStage = await _unitOfWork.StageProgressRepository
                .GetCurrentStageProgressAsync(enrollment.Id);

            if (currentStage == null)
                throw new BadRequestException("No current stage progress found for the enrollment");

            return _mapper.Map<StageProgressDTO>(currentStage);
        }

        public async Task<IEnumerable<StageProgressDTO>> GetCompletedStagesAsync(string applicantId)
        {
            var enrollments = await _unitOfWork.EnrollmentRepository
                .GetByApplicantIdAsync(applicantId);

            var result = new List<StageProgressDTO>();

            foreach (var enrollment in enrollments)
            {
                var completedStages = await _unitOfWork.StageProgressRepository
                    .GetCompletedStagesByEnrollmentIdAsync(enrollment.Id);

                result.AddRange(_mapper.Map<IEnumerable<StageProgressDTO>>(completedStages));
            }

            return result;
        }

        public async Task<IEnumerable<StageProgressDTO>> GetFailedStagesAsync(string applicantId)
        {
            var enrollments = await _unitOfWork.EnrollmentRepository
                .GetByApplicantIdAsync(applicantId);

            var result = new List<StageProgressDTO>();

            foreach (var enrollment in enrollments)
            {
                var failedStages = await _unitOfWork.StageProgressRepository
                    .GetFailedStagesByEnrollmentIdAsync(enrollment.Id);

                result.AddRange(_mapper.Map<IEnumerable<StageProgressDTO>>(failedStages));
            }

            return result;
        }

        // الدوال الجديدة المكملة
        public async Task<StageProgressDTO> CreateNextStageProgressAsync(int enrollmentId, int currentStageId)
        {
            var nextStageProgress = await _unitOfWork.StageProgressRepository
                .CreateNextStageProgressAsync(enrollmentId, currentStageId);

            if (nextStageProgress == null)
                throw new BadRequestException("No next stage available or level completed");

            return _mapper.Map<StageProgressDTO>(nextStageProgress);
        }

        public async Task<int> GetAttemptCountAsync(int enrollmentId, int stageId)
        {
            return await _unitOfWork.StageProgressRepository
                .GetAttemptCountAsync(enrollmentId, stageId);
        }

        public async Task<StageProgressDTO> CreateNewAttemptAsync(int enrollmentId, int stageId)
        {
            // التحقق من أن المرحلة تنتمي للتسجيل الصحيح
            var enrollment = await _unitOfWork.EnrollmentRepository.GetByIdAsync(enrollmentId);
            var stage = await _unitOfWork.StageRepository.GetByIdAsync(stageId);

            if (stage == null || enrollment == null)
                throw new KeyNotFoundException("Enrollment or Stage not found");

            // التحقق من أن المرحلة تسمح بمحاولات متعددة
          //  if (!stage.AllowMultipleAttempts)
           //     throw new BusinessException("This stage does not allow multiple attempts");

            // التحقق من عدم وجود محاولة قيد التقدم
            var existingAttempt = await _unitOfWork.StageProgressRepository
                .GetCurrentStageProgressAsync(enrollmentId);

            if (existingAttempt != null && existingAttempt.StageId == stageId)
                throw new BadRequestException("There is already an active attempt for this stage");

            var newAttempt = await _unitOfWork.StageProgressRepository
                .CreateNewAttemptAsync(enrollmentId, stageId);

            return _mapper.Map<StageProgressDTO>(newAttempt);
        }

        public async Task<IEnumerable<StageProgressDTO>> GetByApplicantIdAsync(string applicantId)
        {
            var enrollments = await _unitOfWork.EnrollmentRepository
                .GetByApplicantIdAsync(applicantId);

            var result = new List<StageProgressDTO>();

            foreach (var enrollment in enrollments)
            {
                var progresses = await _unitOfWork.StageProgressRepository
                    .GetByEnrollmentIdAsync(enrollment.Id);

                result.AddRange(_mapper.Map<IEnumerable<StageProgressDTO>>(progresses));
            }

            return result;
        }
      
}
}
