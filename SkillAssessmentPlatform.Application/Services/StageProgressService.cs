using AutoMapper;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.DTOs.StageProgress;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Certificates_and_Notifications;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces;

namespace SkillAssessmentPlatform.Application.Services
{
    public class StageProgressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly LevelProgressService _levelProgressService;
        private readonly TaskApplicantService _taskApplicantService;

        public StageProgressService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            LevelProgressService levelProgressService,
            TaskApplicantService taskApplicantService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _levelProgressService = levelProgressService;
            _taskApplicantService = taskApplicantService;
        }

        public async Task<StageProgressDTO> GetByIdWithActionStatusAsync(int id)
        {
            var stageProgress = await _unitOfWork.StageProgressRepository.GetDetailedStageProgressAsync(id);
            if (stageProgress == null)
            {
                throw new KeyNotFoundException($"There is no stage progress with id = {id}");
            }

            var dto = _mapper.Map<StageProgressDTO>(stageProgress);
            dto.ApplicantId = stageProgress.LevelProgress.Enrollment.ApplicantId;
            // حساب الـ Action Status
            await SetActionStatusAsync(dto);

            return dto;
        }

        public async Task<IEnumerable<StageProgressDTO>> GetByLevelProgressIdWithActionStatusAsync(int levelProgressId)
        {
            var stageProgresses = await _unitOfWork.StageProgressRepository.GetDetailedByLevelProgressIdAsync(levelProgressId);
            if (stageProgresses == null)
            {
                throw new KeyNotFoundException($"There is no stage progresses in level with id = {levelProgressId}");
            }

            var dtos = _mapper.Map<IEnumerable<StageProgressDTO>>(stageProgresses);

            // حساب الـ Action Status لكل مرحلة
            foreach (var dto in dtos)
            {
                await SetActionStatusAsync(dto);
            }

            return dtos;
        }



        #region status mapping
        private async Task SetActionStatusAsync(StageProgressDTO dto)
        {
            // إذا كانت المرحلة مكتملة بنجاح أو فاشلة
            if (dto.Status == ProgressStatus.Successful)
            {
                dto.ActionStatus = StageActionStatus.Completed;
                return;
            }

            if (dto.Status == ProgressStatus.Failed)
            {
                dto.ActionStatus = StageActionStatus.Failed;
                return;
            }

            // إذا كانت قيد التقدم، نحدد حسب النوع
            switch (dto.StageType)
            {
                case StageType.Exam:
                    await SetExamActionStatusAsync(dto);
                    break;
                case StageType.Interview:
                    await SetInterviewActionStatusAsync(dto);
                    break;
                case StageType.Task:
                    await SetTaskActionStatusAsync(dto);
                    break;
            }
        }

        private async Task SetExamActionStatusAsync(StageProgressDTO dto)
        {
            // البحث عن طلب امتحان للمتقدم في هذه المرحلة
            var examRequest = await _unitOfWork.ExamRequestRepository
                .GetByStageProgressIdAsync(dto.Id);

            if (examRequest == null)
            {
                dto.ActionStatus = StageActionStatus.ReadyToRequest;
                return;
            }

            switch (examRequest.Status)
            {
                case ExamRequestStatus.Pending:
                    dto.ActionStatus = StageActionStatus.RequestPending;
                    dto.AdditionalData = new { ExamRequestId = examRequest.Id };
                    break;
                case ExamRequestStatus.Approved:
                    // التحقق من وجود feedback
                    if (examRequest.FeedbackId.HasValue)
                    {
                        dto.ActionStatus = StageActionStatus.Reviewed;
                        dto.AdditionalData = new { FeedbackId = examRequest.FeedbackId };
                    }
                    else if (examRequest.ScheduledDate > DateTime.UtcNow)
                    {
                        dto.ActionStatus = StageActionStatus.RequestApproved;
                        dto.AdditionalData = new { ExamRequestId = examRequest.Id };
                    }
                    else
                    {
                        dto.ActionStatus = StageActionStatus.ExamCompleted;
                        dto.AdditionalData = new { ExamRequestId = examRequest.Id };
                    }
                    break;
                case ExamRequestStatus.Rejected:
                    dto.ActionStatus = StageActionStatus.RequestRejected;
                    dto.AdditionalData = new { ExamRequestId = examRequest.Id };
                    break;
                case ExamRequestStatus.Canceled:
                    dto.ActionStatus = StageActionStatus.ReadyToRequest;
                    break;
            }
        }

        private async Task SetInterviewActionStatusAsync(StageProgressDTO dto)
        {
            // البحث عن حجز مقابلة للمتقدم في هذه المرحلة
            var interviewBook = await _unitOfWork.InterviewBookRepository
                .GetByStageProgressIdAsync(dto.Id);

            if (interviewBook == null)
            {
                dto.ActionStatus = StageActionStatus.ReadyToBook;
                return;
            }

            switch (interviewBook.Status)
            {
                case InterviewStatus.Pending:
                    dto.ActionStatus = StageActionStatus.BookingPending;
                    dto.AdditionalData = new { InterviewBookId = interviewBook.Id };
                    break;
                case InterviewStatus.Scheduled:
                    dto.ActionStatus = StageActionStatus.BookingScheduled;
                    dto.AdditionalData = new
                    {
                        InterviewBookId = interviewBook.Id,
                        ScheduledDate = interviewBook.ScheduledDate,
                        MeetingLink = interviewBook.MeetingLink
                    };
                    break;
                case InterviewStatus.Completed:
                    if (interviewBook.FeedbackId.HasValue)
                    {
                        dto.ActionStatus = StageActionStatus.Reviewed;
                        dto.AdditionalData = new { FeedbackId = interviewBook.FeedbackId };
                    }
                    else
                    {
                        dto.ActionStatus = StageActionStatus.InterviewCompleted;
                        dto.AdditionalData = new { InterviewBookId = interviewBook.Id };
                    }
                    break;
                case InterviewStatus.Canceled:
                    dto.ActionStatus = StageActionStatus.BookingCanceled;
                    dto.AdditionalData = new { InterviewBookId = interviewBook.Id };
                    break;
            }
        }

        private async Task SetTaskActionStatusAsync(StageProgressDTO dto)
        {
            // البحث عن المهمة المُخصصة للمتقدم
            var taskApplicant = await _unitOfWork.TaskApplicantRepository
                .GetByStageProgressIdAsync(dto.Id);

            if (taskApplicant == null)
            {
                dto.ActionStatus = StageActionStatus.TaskNotAssigned;
                return;
            }

            dto.AdditionalData = new
            {
                TaskApplicantId = taskApplicant.Id,
                DueDate = taskApplicant.DueDate,
                TaskId = taskApplicant.TaskId
            };

            // البحث عن آخر submission
            var latestSubmission = await _unitOfWork.TaskSubmissionRepository
                .GetLatestByTaskApplicantIdAsync(taskApplicant.Id);

            if (latestSubmission == null)
            {
                dto.ActionStatus = StageActionStatus.TaskAssigned;
                return;
            }

            switch (latestSubmission.Status)
            {
                case TaskSubmissionStatus.Submitted:
                case TaskSubmissionStatus.UnderReview:
                    dto.ActionStatus = StageActionStatus.TaskSubmitted;
                    dto.AdditionalData = new
                    {
                        TaskApplicantId = taskApplicant.Id,
                        SubmissionId = latestSubmission.Id,
                        IsLate = false,
                        SubmissionDate = latestSubmission.SubmissionDate
                    };
                    break;
                case TaskSubmissionStatus.Accepted:
                    if (latestSubmission.FeedbackId.HasValue)
                    {
                        dto.ActionStatus = StageActionStatus.Reviewed;
                        dto.AdditionalData = new { FeedbackId = latestSubmission.FeedbackId };
                    }
                    else
                    {
                        dto.ActionStatus = StageActionStatus.TaskAccepted;
                    }
                    break;
                case TaskSubmissionStatus.Rejected:
                    dto.ActionStatus = StageActionStatus.TaskRejected;
                    dto.AdditionalData = new
                    {
                        TaskApplicantId = taskApplicant.Id,
                        SubmissionId = latestSubmission.Id,
                        FeedbackId = latestSubmission.FeedbackId
                    };
                    break;
                case TaskSubmissionStatus.Late:
                    dto.ActionStatus = StageActionStatus.TaskSubmitted;
                    dto.AdditionalData = new
                    {
                        TaskApplicantId = taskApplicant.Id,
                        SubmissionId = latestSubmission.Id,
                        IsLate = true
                    };
                    break;
            }
        }
        #endregion
        public async Task<StageProgressDTO> GetByCurrEnrollmentIdAsync(int enrollmentId)
        {
            var stageProgresses = await _unitOfWork.StageProgressRepository.GetCurrentStageProgressByEnrollmentAsync(enrollmentId);
            if (stageProgresses == null)
            {
                throw new KeyNotFoundException($"There is no stage progresses in level with id = {enrollmentId} ");
            }
            return _mapper.Map<StageProgressDTO>(stageProgresses);
        }

        public async Task<StageProgressDTO> GetCurrentStageProgressAsync(int levelProgressId)
        {
            var stageProgress = await _unitOfWork.StageProgressRepository.GetCurrentStageProgressAsync(levelProgressId);
            if (stageProgress == null)
            {
                throw new KeyNotFoundException($"There is no <In progress> stage in level with id = {levelProgressId}");
            }

            var dto = _mapper.Map<StageProgressDTO>(stageProgress);
            await SetActionStatusAsync(dto);

            return dto;
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

            var stage = await _unitOfWork.StageRepository.GetByIdAsync(stagePById.StageId);
            var trackId = latestStage.Stage.Level.TrackId;
            StageProgress? stageProgress = new();
            // If stage completed successfully and score meets passing criteria
            if (updateDto.Status == ApplicantResultStatus.Passed && updateDto.Score >= stage.PassingScore)
            {
                // update stage progress
                stageProgress = await _unitOfWork.StageProgressRepository.UpdateStatusAsync(
                                                                               stageProgressId,
                                                                               ProgressStatus.Successful,
                                                                               (int)updateDto.Score);
                await _unitOfWork.ExaminerLoadRepository.DecrementWorkloadAsync(
                                                                            stageProgress.ExaminerId,
                                                                            MapLoad(latestStage.Stage.Type));
                // free examiner workload
                var freeExaminerId = await _unitOfWork.ExaminerRepository.GetAvailableExaminerAsync(trackId, MapLoad(stage.Type));
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

                    var levelProgressId = stageProgress.LevelProgressId;
                    var updateLevelDto = new UpdateLevelStatusDTO
                    {
                        Status = ProgressStatus.Successful
                    };

                    await _levelProgressService.UpdateStatusAsync(levelProgressId, updateLevelDto);
                }
                if (nextStageProgress.Stage.Type == StageType.Task)
                {
                    await _taskApplicantService.AssignRandomTaskAsync(
                                    new AssignTaskDTO { StageProgressId = nextStageProgress.Id });
                }

            }
            else if (updateDto.Status == ApplicantResultStatus.Failed)
            {
                // update stage progress
                stageProgress = await _unitOfWork.StageProgressRepository.UpdateStatusAsync(
                                                                               stageProgressId,
                                                                               ProgressStatus.Failed,
                                                                               (int)updateDto.Score);
            }
            else if (updateDto.Status == ApplicantResultStatus.ResubmissionAllowed)
            {
                //No need for update stage progress

            }

            return _mapper.Map<StageProgressDTO>(stageProgress);
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

                //// No next level, track completed
                if (result == null)
                {
                    await _unitOfWork.EnrollmentRepository.UpdateStatusAsync(levelProgress.EnrollmentId, EnrollmentStatus.Completed);
                }


                await _unitOfWork.SaveChangesAsync();
            }

            return _mapper.Map<LevelProgressDTO>(levelProgress);
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
            var type = MapLoad(stage.Type);
            var canTakeMore = await _unitOfWork.ExaminerLoadRepository.CanTakeMoreLoadAsync(
                assignDto.ExaminerId,
                type);

            if (!canTakeMore)
                throw new BadRequestException("Examiner has reached maximum workload for this stage type");

            // Assign examiner
            var updatedStageProgress = await _unitOfWork.StageProgressRepository.AssignExaminerAsync(
                stageProgressId,
                assignDto.ExaminerId);

            // Update examiner's current workload
            await _unitOfWork.ExaminerLoadRepository.IncrementWorkloadAsync(
                assignDto.ExaminerId,
                type);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<StageProgressDTO>(updatedStageProgress);
        }
        private LoadType MapLoad(StageType stageType)
        {
            var type = LoadType.Task;
            if (stageType == StageType.Exam) { type = LoadType.Exam; }
            else if (stageType == StageType.Interview) { type = LoadType.Interview; }
            return type;
        }
        public async Task<StageProgressDTO> GetCurrentStageForApplicantAsync(string applicantId)
        {
            // Get the latest active enrollment for the applicant
            var enrollment = await _unitOfWork.EnrollmentRepository
           .GetLatestActiveEnrollmentAsync(applicantId);

            if (enrollment == null)
                throw new BadRequestException("No active enrollment found for the applicant");

            var levelProgress = await _unitOfWork.LevelProgressRepository
                .GetCurrentLevelProgressAsync(enrollment.Id);
            if (levelProgress == null)
                throw new BadRequestException("No active level found for the applicant");

            var currentStage = await _unitOfWork.StageProgressRepository
                .GetCurrentStageProgressAsync(levelProgress.Id);

            if (currentStage == null)
                throw new BadRequestException("No current stage progress found for the enrollment");

            var dto = _mapper.Map<StageProgressDTO>(currentStage);
            await SetActionStatusAsync(dto);

            return dto;
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
        public async Task<int> GetAttemptCountAsync(int stageId)
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

            var freeExaminerId = await _unitOfWork.ExaminerRepository.GetAvailableExaminerAsync(enrollment.TrackId, MapLoad(stage.Type));
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