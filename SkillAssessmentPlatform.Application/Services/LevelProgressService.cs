using AutoMapper;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Certificates_and_Notifications;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Interfaces;

namespace SkillAssessmentPlatform.Application.Services
{
    public class LevelProgressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly TaskApplicantService _taskApplicantService;
        public LevelProgressService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
             TaskApplicantService taskApplicantService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _taskApplicantService = taskApplicantService;
        }

        public async Task<LevelProgressDTO> GetByIdAsync(int id)
        {
            var levelProgress = await _unitOfWork.LevelProgressRepository.GetByIdAsync(id);
            return _mapper.Map<LevelProgressDTO>(levelProgress);
        }

        public async Task<IEnumerable<LevelProgressDTO>> GetByEnrollmentIdAsync(int enrollmentId)
        {
            var levelProgresses = await _unitOfWork.LevelProgressRepository.GetByEnrollmentIdAsync(enrollmentId);

            var levelProgressDTOs = new List<LevelProgressDTO>();

            foreach (var levelProgress in levelProgresses)
            {
                var lastStageProgress = await _unitOfWork.StageProgressRepository.GetLatestSPinLPAsync(levelProgress.Id);

                var dto = _mapper.Map<LevelProgressDTO>(levelProgress);
                dto.StagesProgressesCount = lastStageProgress?.Stage.Order ?? 0;

                levelProgressDTOs.Add(dto);
            }

            return levelProgressDTOs;
        }

        public async Task<LevelProgressDTO> GetCurrentLevelProgressAsync(int enrollmentId)
        {
            var levelProgress = await _unitOfWork.LevelProgressRepository.GetCurrentLevelProgressAsync(enrollmentId);
            return _mapper.Map<LevelProgressDTO>(levelProgress);
        }


        public async Task<LevelProgressDTO> UpdateStatusAsync(int levelProgressId, UpdateLevelStatusDTO updateDto)
        {
            var levelProgress = await _unitOfWork.LevelProgressRepository.UpdateStatusAsync(levelProgressId, updateDto.Status);

            // If level completed successfully, create progress for next level
            if (updateDto.Status == ProgressStatus.Successful)
            {
                var enrollment = await _unitOfWork.EnrollmentRepository.GetByIdAsync(levelProgress.EnrollmentId);


                var certificate = new AppCertificate
                {
                    ApplicantId = enrollment.ApplicantId,
                    LeveProgressId = levelProgress.Id,
                    IssueDate = DateTime.UtcNow,
                    VerificationCode = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper()
                };
                await _unitOfWork.AppCertificateRepository.AddAsync(certificate);


                var result = await _unitOfWork.LevelProgressRepository.CreateNextLevelProgressAsync(
                    levelProgress.EnrollmentId,
                    levelProgress.LevelId);
                var firstStage = await _unitOfWork.StageRepository.GetFirstStageByLevelIdAsync(result.LevelId);
                if (firstStage != null)
                {
                    var freeExaminerId = await _unitOfWork.ExaminerRepository.GetAvailableExaminerAsync(enrollment.TrackId, MapLoad(firstStage.Type));
                    if (freeExaminerId == null)
                        throw new InvalidOperationException("No available examiner found for this stage");

                    var stageProgress = new StageProgress
                    {
                        LevelProgressId = result.Id,
                        StageId = firstStage.Id,
                        Status = ProgressStatus.InProgress,
                        StartDate = DateTime.Now,
                        Attempts = 1,
                        ExaminerId = freeExaminerId.ToString()
                    };
                    await _unitOfWork.ExaminerLoadRepository.IncrementWorkloadAsync(freeExaminerId, MapLoad(firstStage.Type));
                    await _unitOfWork.StageProgressRepository.AddAsync(stageProgress);
                    await _unitOfWork.SaveChangesAsync();
                    if (firstStage.Type == StageType.Task)
                    {
                        try
                        {
                            await _taskApplicantService.AssignRandomTaskAsync(
                                   new AssignTaskDTO { StageProgressId = stageProgress.Id });
                        }
                        catch (Exception ex)
                        {
                            //   _logger.LogError(ex, "Failed to assign task for StageProgressId: {StageProgressId}", stageProgress.Id);

                        }

                    }
                }

                //// No next level, track completed
                if (result == null)
                {
                    await _unitOfWork.EnrollmentRepository.UpdateStatusAsync(levelProgress.EnrollmentId, EnrollmentStatus.Completed);
                }


                await _unitOfWork.SaveChangesAsync();
            }

            return _mapper.Map<LevelProgressDTO>(levelProgress);
        }

        private LoadType MapLoad(StageType stageType)
        {
            var type = LoadType.Task;
            if (stageType == StageType.Exam) { type = LoadType.Exam; }
            else if (stageType == StageType.Interview) { type = LoadType.Interview; }
            return type;
        }

        public async Task<IEnumerable<LevelProgressDTO>> GetByApplicantIdAsync(string applicantId)
        {
            // Get all enrollments for this applicant
            var enrollments = await _unitOfWork.EnrollmentRepository.GetByApplicantIdAsync(applicantId);

            var result = new List<LevelProgressDTO>();

            foreach (var enrollment in enrollments)
            {
                var levelProgresses = await _unitOfWork.LevelProgressRepository.GetByEnrollmentIdAsync(enrollment.Id);
                result.AddRange(_mapper.Map<IEnumerable<LevelProgressDTO>>(levelProgresses));
            }

            return result;
        }
    }
}
