using AutoMapper;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            if (stageProgress == null)
            {
                throw new KeyNotFoundException($"KeyNotFoundException : There is no stage progress with id = {id} ");
            }
            return _mapper.Map<StageProgressDTO>(stageProgress);
        }

        public async Task<IEnumerable<StageProgressDTO>> GetByLevelProgressIdAsync(int levelprogressId)
        {
            var stageProgresses = await _unitOfWork.StageProgressRepository.GetByLevelProgressIdAsync(levelprogressId);
            if (stageProgresses == null)
            {
                throw new KeyNotFoundException($"There is no stage progresses in level with id = {levelprogressId} ");
            }
            return _mapper.Map<IEnumerable<StageProgressDTO>>(stageProgresses);
        }
        public async Task<StageProgressDTO> GetByCurrEnrollmentIdAsync(int enrollmentId)
        {
            var stageProgresses = await _unitOfWork.StageProgressRepository.GetCurrentStageProgressByEnrollmentAsync(enrollmentId);
            if (stageProgresses == null)
            {
                throw new KeyNotFoundException($"There is no stage progresses in level with id = {enrollmentId} ");
            }
            return _mapper.Map<StageProgressDTO>(stageProgresses);
        }

        public async Task<StageProgressDTO> GetCurrentStageProgressAsync(int levelprogressId)
        {
            var stageProgress = await _unitOfWork.StageProgressRepository.GetCurrentStageProgressAsync(levelprogressId);
            if(stageProgress == null)
            {
                throw new KeyNotFoundException($"There is no <In progress> stage in level with id = {levelprogressId} ");
            }
            return _mapper.Map<StageProgressDTO>(stageProgress);
        }
        //==> this method may be it can be combined with other endpoint (after examiner give thw applicant score)
        public async Task<StageProgressDTO> UpdateStatusAsync(int stageProgressId, UpdateStageStatusDTO updateDto)
        {
            var stagePById = await _unitOfWork.StageProgressRepository.GetByIdAsync(stageProgressId);
            var latestStage = await _unitOfWork.StageProgressRepository.GetLatestSPinLPAsync(stagePById.LevelProgressId);
            
            if (stagePById.Status != ProgressStatus.InProgress)
            {
                throw new BadRequestException($"can not update this stage progress status");
            }
            
            if (latestStage.Id != stageProgressId) 
            {
                throw new BadRequestException($"can not update this stage progress, it's not the latest ");
            }

            var stageProgress = await _unitOfWork.StageProgressRepository.UpdateStatusAsync(
                stageProgressId,
                updateDto.Status,
                updateDto.Score);

            var stage = await _unitOfWork.StageRepository.GetByIdAsync(stageProgress.StageId);

            // If stage completed successfully and score meets passing criteria
            if (updateDto.Status == ProgressStatus.Successful && updateDto.Score >= stage.PassingScore)
            {
                var freeExaminerId = await _unitOfWork.ExaminerRepository.GetAvailableExaminerAsync(stage.Type);
                if (freeExaminerId == null)
                    throw new InvalidOperationException("No available examiner found for this stage");

                //Create progress for next stage
                var nextStageProgress = await _unitOfWork.StageProgressRepository.CreateNextStageProgressAsync(
                    stageProgress.LevelProgressId,
                    stageProgress.StageId,
                    freeExaminerId); 
               
                // If no next stage, level is completed
                if (nextStageProgress == null)
                {
                    //Update level status to successful
                    var levelProgressId = stageProgress.LevelProgressId;
                    var levelProgress = await _unitOfWork.LevelProgressRepository.GetByIdAsync(levelProgressId);
                    await _unitOfWork.LevelProgressRepository.UpdateStatusAsync(levelProgressId, ProgressStatus.Successful);

                    // add next level in level progress
                   var result = await _unitOfWork.LevelProgressRepository.CreateNextLevelProgressAsync(
                   levelProgress.EnrollmentId,
                   levelProgress.LevelId);
                   if (result == null)  //// No next level, track completed
                   {
                        await _unitOfWork.EnrollmentRepository.UpdateStatusAsync(levelProgress.EnrollmentId, EnrollmentStatus.Completed);
                        /* >>> ---
                         * / Create certificate 
                        var certificate = new AppCertificate
                        {
                            ApplicantId = enrollment.ApplicantId,
                            LevelProgressID = levelProgress.Id,
                            IssueDate = DateTime.UtcNow,
                            VerificationCode = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper()
                        };

                        await _context.Certificates.AddAsync(certificate);
                        /*/
                   }

                }
            }
            else if (updateDto.Status == ProgressStatus.Failed)
            {
               
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

            var levelprogress = await _unitOfWork.LevelProgressRepository
                .GetCurrentLevelProgressAsync(enrollment.Id);
            if (levelprogress == null)
                throw new BadRequestException("No active level found for the applicant");

            var currentStage = await _unitOfWork.StageProgressRepository
                .GetCurrentStageProgressAsync(levelprogress.Id);

            if (currentStage == null)
                throw new BadRequestException("No current stage progress found for the enrollment");

            return _mapper.Map<StageProgressDTO>(currentStage);
        }
        public async Task<IEnumerable<StageProgressDTO>> GetByApplicantIdAsync(string applicantId)
        {
            var enrollments = await _unitOfWork.EnrollmentRepository
                .GetByApplicantIdAsync(applicantId);

            var result = new List<StageProgressDTO>();

            foreach (var enrollment in enrollments)
            {
                var levels = await _unitOfWork.LevelProgressRepository.GetByEnrollmentIdAsync(enrollment.Id);

                foreach (var lp in levels)
                {
                    var progresses = await _unitOfWork.StageProgressRepository
                    .GetByLevelProgressIdAsync(lp.Id);

                    result.AddRange(_mapper.Map<IEnumerable<StageProgressDTO>>(progresses));
                }
            }

            return result;
        }
        public async Task<IEnumerable<StageProgressDTO>> GetCompletedStagesAsync(string applicantId)
        {
            var enrollments = await _unitOfWork.EnrollmentRepository
                .GetByApplicantIdAsync(applicantId);

            var result = new List<StageProgressDTO>();

            foreach (var enrollment in enrollments)
            {
                var levels = await _unitOfWork.LevelProgressRepository.GetCompletedLevelsByEnrollmentIdAsync(enrollment.Id);

                foreach (var lp in levels)
                {
                    var completedStages = await _unitOfWork.StageProgressRepository
                    .GetCompletedStagesLPIdAsync(lp.Id);
                    result.AddRange(_mapper.Map<IEnumerable<StageProgressDTO>>(completedStages));

                }
               
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
        /*
        public async AppTask<StageProgressDTO> CreateNextStageProgressAsync(int enrollmentId, int currentStageId)
        {
            var nextStageProgress = await _unitOfWork.StageProgressRepository
                .CreateNextStageProgressAsync(enrollmentId, currentStageId);

            if (nextStageProgress == null)
                throw new BadRequestException("No next stage available or level completed");

            return _mapper.Map<StageProgressDTO>(nextStageProgress);
        }
        */
        public async Task<int> GetAttemptCountAsync( int stageId)
        {
            return await _unitOfWork.StageProgressRepository
                .GetAttemptCountAsync(stageId);
        }

        public async Task<StageProgressDTO> CreateNewAttemptAsync(int enrollmentId, int stageId)
        {
            // التحقق من أن المرحلة تنتمي للتسجيل الصحيح
            var enrollment = await _unitOfWork.EnrollmentRepository.GetByIdAsync(enrollmentId);
            var stage = await _unitOfWork.StageRepository.GetByIdAsync(stageId);

            if (stage == null || enrollment == null)
                throw new KeyNotFoundException("Enrollment or Stage not found");

            // check the no. of allowed attempts and existed attempts
            var attempts = await _unitOfWork.StageProgressRepository.GetAttemptCountAsync(stageId);
            if (stage.NoOfAttempts <= attempts)
                throw new BadRequestException("This stage does not allow more attempts");

            // التحقق من عدم وجود محاولة قيد التقدم
            var existingAttempt = await _unitOfWork.StageProgressRepository
                .GetCurrentStageProgressByEnrollmentAsync(enrollmentId);
            
            if (existingAttempt != null && existingAttempt.StageId == stageId)
                throw new BadRequestException("There is already an active attempt for this stage");
            
            var freeExaminerId = await _unitOfWork.ExaminerRepository.GetAvailableExaminerAsync(stage.Type);
            if (freeExaminerId == null)
                throw new InvalidOperationException("No available examiner found for this stage");

            // get level progress ID 
            var levelProgressId = await _unitOfWork.StageProgressRepository.GetLevelProgressIdofStageAsync(stage.Id);
           
            var newAttempt = await _unitOfWork.StageProgressRepository
                .CreateNewAttemptAsync(levelProgressId, stageId, freeExaminerId);

            return _mapper.Map<StageProgressDTO>(newAttempt);
        }

}
}
