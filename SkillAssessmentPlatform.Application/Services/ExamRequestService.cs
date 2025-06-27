using AutoMapper;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Application.Abstract;
using SkillAssessmentPlatform.Application.DTOs.ExamReques.Input;
using SkillAssessmentPlatform.Application.DTOs.ExamReques.Output;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces;

namespace SkillAssessmentPlatform.Application.Services
{
    public class ExamRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ExamRequestService> _logger;
        private readonly IEmailService _emailService;
        private readonly NotificationService _notificationService;
        private readonly StageProgressService _stageProgressService;

        public ExamRequestService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ExamRequestService> logger,
            IEmailService emailService,
            NotificationService notificationService,
            StageProgressService stageProgressService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _emailService = emailService;
            _notificationService = notificationService;
            _stageProgressService = stageProgressService;
        }

        public async Task<ExamRequestDTO> CreateExamRequestAsync(ExamRequestCreateDTO requestDTO)
        {
            // Validate exam exists and is active
            var exam = await _unitOfWork.ExamRepository.GetByStageIdAsync(requestDTO.StageId);
            if (exam == null || !exam.IsActive)
                throw new KeyNotFoundException($"Exam with id {requestDTO.StageId} not found or is inactive");

            // Validate applicant exists
            var applicant = await _unitOfWork.ApplicantRepository.GetByIdAsync(requestDTO.ApplicantId);
            if (applicant == null)
                throw new KeyNotFoundException($"Applicant with id {requestDTO.ApplicantId} not found");

            // Check if there's already a pending request for this exam and applicant
            var existingRequests = await _unitOfWork.ExamRequestRepository.GetByApplicantIdAsync(requestDTO.ApplicantId);
            if (existingRequests.Any(er => er.Exam.StageId == requestDTO.StageId && er.Status == ExamRequestStatus.Pending))
                throw new BadRequestException("There is already a pending request for this exam");
            var stageProgress = await _unitOfWork.StageProgressRepository.GetByIdAsync(requestDTO.StageProgressId);
            if (stageProgress == null)
                throw new Exception("StageProgress not found.");


            // Create new exam request
            var examRequest = new ExamRequest
            {
                ExamId = exam.Id,
                ApplicantId = requestDTO.ApplicantId,
                StageProgressId = stageProgress.Id,
                Status = ExamRequestStatus.Pending,
                ScheduledDate = DateTime.UtcNow, // will be updated when approved
                Instructions = requestDTO.Instructions,

            };

            await _unitOfWork.ExamRequestRepository.AddAsync(examRequest);
            await _unitOfWork.SaveChangesAsync();

            // Get the stage details to notify the senior examiner
            var stage = await _unitOfWork.StageRepository.GetByIdAsync(exam.StageId);
            var level = await _unitOfWork.LevelRepository.GetByIdAsync(stage.LevelId);
            var track = await _unitOfWork.TrackRepository.GetByIdAsync(level.TrackId);

            // Notify senior examiner if available
            if (!string.IsNullOrEmpty(track.SeniorExaminerID))
            {
                await _notificationService.SendNotificationAsync(
                    track.SeniorExaminerID,
                     NotificationType.NewExamRequest,
                    $"New exam request for {applicant.FullName} in {stage.Name}"
                   );
            }

            return _mapper.Map<ExamRequestDTO>(examRequest);
        }

        public async Task<ExamRequestDTO> GetExamRequestByIdAsync(int id)
        {
            var examRequest = await _unitOfWork.ExamRequestRepository.GetWithApplicantAndExamAsync(id);
            if (examRequest == null)
                throw new KeyNotFoundException($"ExamRequest with id {id} not found");

            return _mapper.Map<ExamRequestDTO>(examRequest);
        }
        public async Task<ExamRequestInfoApplicantDTO> GetExamRequestInfoByIdAsync(int id)
        {
            var examRequest = await _unitOfWork.ExamRequestRepository.GetWithApplicantAndExamAsync(id);
            if (examRequest == null)
                throw new KeyNotFoundException($"ExamRequest with id {id} not found");

            return _mapper.Map<ExamRequestInfoApplicantDTO>(examRequest);
        }

        public async Task<IEnumerable<ExamRequestDTO>> GetExamRequestsByApplicantIdAsync(string applicantId)
        {
            var examRequests = await _unitOfWork.ExamRequestRepository.GetByApplicantIdAsync(applicantId);
            return _mapper.Map<IEnumerable<ExamRequestDTO>>(examRequests);
        }

        public async Task<IEnumerable<ExamRequestDTO>> GetExamRequestsByStageIdAsync(int stageId)
        {
            var examRequests = await _unitOfWork.ExamRequestRepository.GetByStageIdAsync(stageId);
            return _mapper.Map<IEnumerable<ExamRequestDTO>>(examRequests);
        }

        public async Task<IEnumerable<ExamRequestDTO>> GetPendingExamRequestsByStageIdAsync(int stageId)
        {
            var examRequests = await _unitOfWork.ExamRequestRepository.GetPendingRequestsByStageIdAsync(stageId);
            return _mapper.Map<IEnumerable<ExamRequestDTO>>(examRequests);
        }

        public async Task<PendingExamRequestSummaryDTO> GetPendingExamRequestsSummaryAsync(string trackId)
        {
            var pendingCounts = await _unitOfWork.ExamRequestRepository
                                        .GetPendingExamRequestCountsByStageAsync(trackId);
            var result = new PendingExamRequestSummaryDTO();

            // If there are no pending requests, return empty summary
            if (pendingCounts.Count == 0)
                return result;

            // Get stage details for stages with pending requests
            foreach (var kvp in pendingCounts)
            {
                var stageId = kvp.Key;
                var count = kvp.Value;

                var stage = await _unitOfWork.StageRepository.GetByIdAsync(stageId);
                if (stage != null)
                {
                    var level = await _unitOfWork.LevelRepository.GetByIdAsync(stage.LevelId);

                    result.Stages.Add(new StageExamRequestsDTO
                    {
                        StageId = stageId,
                        StageName = stage.Name,
                        LevelName = level.Name,
                        PendingRequestsCount = count
                    });
                }
            }

            return result;
        }
        /*
         * Validate input
         * Validate request 
         * Update request
         * find & notify examiner
         *notify applicant & send email
         */
        public async Task<ExamRequestDTO> ApproveExamRequestAsync(int requestId, ExamRequestUpdateDTO updateDTO)
        {
            if (!updateDTO.ScheduledDate.HasValue || updateDTO.ScheduledDate <= DateTime.UtcNow.AddMinutes(1))
                throw new BadRequestException("Scheduled date must be at least 1 minute in the future");

            if (string.IsNullOrWhiteSpace(updateDTO.Instructions))
                throw new BadRequestException("Instructions are required");

            var examRequest = await _unitOfWork.ExamRequestRepository.GetWithApplicantAndExamAsync(requestId);
            if (examRequest == null)
                throw new KeyNotFoundException($"ExamRequest with id {requestId} not found");

            if (examRequest.Status != ExamRequestStatus.Pending)
                throw new BadRequestException("Only pending requests can be approved");

            examRequest = await _unitOfWork.ExamRequestRepository.UpdateStatusAsync(
                requestId,
                ExamRequestStatus.Approved,
                updateDTO.Instructions,
                updateDTO.ScheduledDate);

            // find the assigned examiner
            var stageProgress = await _unitOfWork.StageProgressRepository.GetByApplicantAndStageAsync(
                examRequest.ApplicantId,
                examRequest.Exam.StageId);

            if (stageProgress == null)
                throw new KeyNotFoundException($"StageProgress not found for applicant {examRequest.ApplicantId} and stage {examRequest.Exam.StageId}");

            await SendExamApprovalEmailAsync(examRequest);

            await _notificationService.SendNotificationAsync(
                examRequest.ApplicantId,
                 NotificationType.ExamRequestApprove,
                $"Your exam request for {examRequest.Exam.Stage.Name} has been approved. Scheduled for {examRequest.ScheduledDate:yyyy-MM-dd HH:mm}"
               );

            if (!string.IsNullOrEmpty(stageProgress.ExaminerId))
            {
                await _notificationService.SendNotificationAsync(
                    stageProgress.ExaminerId,
                    NotificationType.ExamRequestApprove,
                    $"New exam scheduled for {examRequest.Applicant.FullName} on {examRequest.ScheduledDate:yyyy-MM-dd HH:mm}"
                    );
            }

            return _mapper.Map<ExamRequestDTO>(examRequest);
        }
        /*
        * Validate input
        * Validate request 
        * Update request
        * notify applicant & send email
        */
        public async Task<ExamRequestDTO> RejectExamRequestAsync(int requestId, string message = null)
        {
            var examRequest = await _unitOfWork.ExamRequestRepository.GetWithApplicantAndExamAsync(requestId);
            if (examRequest == null)
                throw new KeyNotFoundException($"ExamRequest with id {requestId} not found");

            if (examRequest.Status != ExamRequestStatus.Pending)
                throw new BadRequestException("Only pending requests can be rejected");

            examRequest = await _unitOfWork.ExamRequestRepository.UpdateStatusAsync(
                requestId,
                ExamRequestStatus.Rejected,
                message);
            var sp = examRequest.Exam.Stage.StageProgresses.OrderByDescending(x => x.StartDate).FirstOrDefault();
            if (sp == null)
            {
                throw new Exception("error in update applicant progress");
            }
            // تحديث الستيج بروغريس
            /*
            await _stageProgressService.UpdateStatusAsync(sp.Id,
                new UpdateStageStatusDTO { Score = 0, Status = ApplicantResultStatus.Failed });
            */
            await SendExamRejectionEmailAsync(examRequest, message);

            // Create notification for applicant
            await _notificationService.SendNotificationAsync(
                examRequest.ApplicantId,
                NotificationType.ExamRequestReject,
                $"Your exam request for {examRequest.Exam.Stage.Name} has been rejected."
                );

            return _mapper.Map<ExamRequestDTO>(examRequest);
        }
        /*
         * if new status is Approve
         * validate scheduled date + instructions then 
         * change all requests with provided IDs
         * then send notifications for applicant am=nd supervisor
         * if reject
         * change all requests with provided IDs
         * notify applicant 
         * */

        public async Task<IEnumerable<ExamRequestDTO>> BulkUpdateExamRequestsAsync(ExamRequestBulkUpdateDTO bulkUpdateDTO)
        {
            if (bulkUpdateDTO.RequestIds == null || !bulkUpdateDTO.RequestIds.Any())
                throw new BadRequestException("No request IDs provided");

            if (bulkUpdateDTO.Status == ExamRequestStatus.Approved)
            {
                if (!bulkUpdateDTO.ScheduledDate.HasValue || bulkUpdateDTO.ScheduledDate <= DateTime.UtcNow.AddMinutes(1))
                    throw new BadRequestException("Scheduled date is required and must be in the future");

                if (string.IsNullOrWhiteSpace(bulkUpdateDTO.Instructions))
                    throw new BadRequestException("Instructions are required for approved requests");
            }

            var updatedRequests = new List<ExamRequest>();

            foreach (var requestId in bulkUpdateDTO.RequestIds)
            {
                var examRequest = await _unitOfWork.ExamRequestRepository.GetWithApplicantAndExamAsync(requestId);
                if (examRequest == null || examRequest.Status != ExamRequestStatus.Pending)
                    continue;

                // ======================== APPROVED ========================
                if (bulkUpdateDTO.Status == ExamRequestStatus.Approved)
                {
                    examRequest = await _unitOfWork.ExamRequestRepository.UpdateStatusAsync(
                        requestId,
                        ExamRequestStatus.Approved,
                        bulkUpdateDTO.Instructions,
                        bulkUpdateDTO.ScheduledDate);

                    // Send email to applicant
                    await SendExamApprovalEmailAsync(examRequest);

                    // Get the stage progress to find the assigned examiner
                    var stageProgress = await _unitOfWork.StageProgressRepository.GetByApplicantAndStageAsync(
                        examRequest.ApplicantId,
                        examRequest.Exam.StageId);

                    // Create notification for applicant
                    await _notificationService.SendNotificationAsync(
                        examRequest.ApplicantId,
                        NotificationType.ExamRequestApprove,
                        $"Your exam request for {examRequest.Exam.Stage.Name} has been approved. Scheduled for {examRequest.ScheduledDate:yyyy-MM-dd HH:mm}"
                        );

                    // If an examiner is assigned, create a notification for them as well
                    if (stageProgress != null && !string.IsNullOrEmpty(stageProgress.ExaminerId))
                    {
                        await _notificationService.SendNotificationAsync(
                            stageProgress.ExaminerId,
                            NotificationType.ExamRequestApprove,
                            $"New exam scheduled for {examRequest.Applicant.FullName} on {examRequest.ScheduledDate:yyyy-MM-dd HH:mm}"
                            );
                    }
                }
                // ======================== REJECTED ========================
                else if (bulkUpdateDTO.Status == ExamRequestStatus.Rejected)
                {
                    examRequest = await _unitOfWork.ExamRequestRepository.UpdateStatusAsync(
                        requestId,
                        ExamRequestStatus.Rejected,
                        bulkUpdateDTO.Instructions);

                    // Send email to applicant
                    await SendExamRejectionEmailAsync(examRequest, bulkUpdateDTO.Instructions);

                    // Create notification for applicant
                    await _notificationService.SendNotificationAsync(
                        examRequest.ApplicantId,
                        NotificationType.ExamRequestReject,
                        $"Your exam request for {examRequest.Exam.Stage.Name} has been rejected."
                        );
                }

                updatedRequests.Add(examRequest);
            }

            return _mapper.Map<IEnumerable<ExamRequestDTO>>(updatedRequests);
        }

        public async Task<bool> CheckExaminerToReviewExamsAsync()
        {
            try
            {
                // Get all approved exam requests where the scheduled date is today or in the past
                var examRequests = await _unitOfWork.ExamRequestRepository.GetAllAsync();
                var requestsToCheck = examRequests
                    .Where(er =>
                        er.Status == ExamRequestStatus.Approved &&
                        er.ScheduledDate.Date <= DateTime.Today &&
                        er.FeedbackId == null)
                    .ToList();

                foreach (var request in requestsToCheck)
                {
                    // Get the stage progress to find the assigned examiner
                    var stageProgress = await _unitOfWork.StageProgressRepository.GetByApplicantAndStageAsync(
                        request.ApplicantId,
                        request.Exam.StageId);

                    if (stageProgress != null && !string.IsNullOrEmpty(stageProgress.ExaminerId))
                    {
                        // Notify the examiner that they need to review the exam
                        await _notificationService.SendNotificationAsync(
                            stageProgress.ExaminerId,
                            NotificationType.ReviewExam,
                            $"Please review the exam for {request.Applicant.FullName} that was scheduled for {request.ScheduledDate:yyyy-MM-dd}"
                            );
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking examiners to review exams");
                return false;
            }
        }

        #region Private Helper Methods

        private async Task SendExamApprovalEmailAsync(ExamRequest examRequest)
        {
            var applicant = examRequest.Applicant;
            var exam = examRequest.Exam;
            var stage = exam.Stage;

            string subject = $"Your Exam Request for {stage.Name} has been approved";
            string body = $@"
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; margin: 20px; }}
                    .container {{ padding: 20px; border: 1px solid #ddd; border-radius: 5px; }}
                    .header {{ background-color: #f5f5f5; padding: 10px; margin-bottom: 20px; }}
                    .content {{ margin-bottom: 20px; }}
                    .footer {{ font-size: 12px; color: #666; margin-top: 30px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h2>Exam Request Approved</h2>
                    </div>
                    <div class='content'>
                        <p>Dear {applicant.FullName},</p>
                        <p>Your request for the exam <strong>{stage.Name}</strong> has been approved.</p>
                        <p><strong>Scheduled Date:</strong> {examRequest.ScheduledDate:yyyy-MM-dd HH:mm}</p>
                        <p><strong>Duration:</strong> {exam.DurationMinutes} minutes</p>
                        <p><strong>Instructions:</strong></p>
                        <p>{examRequest.Instructions}</p>
                        <p>Please make sure you are prepared and available at the scheduled time.</p>
                        <p>Good luck!</p>
                    </div>
                    <div class='footer'>
                        <p>This is an automated message. Please do not reply to this email.</p>
                    </div>
                </div>
            </body>
            </html>";

            await _emailService.SendEmailAsync(applicant.Email, subject, body);
        }

        private async Task SendExamRejectionEmailAsync(ExamRequest examRequest, string message)
        {
            var applicant = examRequest.Applicant;
            var exam = examRequest.Exam;
            var stage = exam.Stage;

            string subject = $"Your Exam Request for {stage.Name} has been rejected";
            string body = $@"
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; margin: 20px; }}
                    .container {{ padding: 20px; border: 1px solid #ddd; border-radius: 5px; }}
                    .header {{ background-color: #f5f5f5; padding: 10px; margin-bottom: 20px; }}
                    .content {{ margin-bottom: 20px; }}
                    .footer {{ font-size: 12px; color: #666; margin-top: 30px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h2>Exam Request Rejected</h2>
                    </div>
                    <div class='content'>
                        <p>Dear {applicant.FullName},</p>
                        <p>Unfortunately, your request for the exam <strong>{stage.Name}</strong> has been rejected.</p>
                        {(string.IsNullOrEmpty(message) ? "" : $"<p><strong>Reason:</strong> {message}</p>")}
                        <p>If you have any questions, please contact your track coordinator.</p>
                    </div>
                    <div class='footer'>
                        <p>This is an automated message. Please do not reply to this email.</p>
                    </div>
                </div>
            </body>
            </html>";

            await _emailService.SendEmailAsync(applicant.Email, subject, body);
        }

        #endregion

    }
}
