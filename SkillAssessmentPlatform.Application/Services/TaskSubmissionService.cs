using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Interfaces;

namespace SkillAssessmentPlatform.Application.Services
{
    public class TaskSubmissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly NotificationService _notificationService;

        public TaskSubmissionService(IUnitOfWork unitOfWork, NotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task<TaskSubmissionDTO> SubmitTaskAsync(CreateTaskSubmissionDTO dto)
        {
            var taskApplicant = await _unitOfWork.TaskApplicantRepository.GetByIdAsync(dto.TaskApplicantId)
                                ?? throw new KeyNotFoundException("TaskApplicant not found");

            var submission = new TaskSubmission
            {
                TaskApplicantId = dto.TaskApplicantId,
                SubmissionUrl = dto.SubmissionUrl,
                SubmissionDate = DateTime.UtcNow // يتم تعيينها تلقائيًا هنا
            };

            await _unitOfWork.TaskSubmissionRepository.AddAsync(submission);
            await _unitOfWork.SaveChangesAsync();

            // Get StageProgress → ExaminerId
            var stageProgress = await _unitOfWork.StageProgressRepository.GetByIdAsync(taskApplicant.StageProgressId);
            if (stageProgress == null)
                throw new Exception("StageProgress not found");

            // Send Notification to Examiner
            await _notificationService.SendNotificationAsync(
                stageProgress.ExaminerId,
                NotificationType.TaskSubmitted,
                $"Applicant submitted a task on {submission.SubmissionDate:yyyy-MM-dd}");

            return new TaskSubmissionDTO
            {
                Id = submission.Id,
                TaskApplicantId = submission.TaskApplicantId,
                SubmissionUrl = submission.SubmissionUrl,
                SubmissionDate = submission.SubmissionDate
            };
        }
    }
}
