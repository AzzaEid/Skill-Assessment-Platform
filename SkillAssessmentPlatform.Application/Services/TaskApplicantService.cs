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
    public class TaskApplicantService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskApplicantService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TaskApplicantDTO> AssignRandomTaskAsync(AssignTaskDTO dto)
        {
            // 1. جلب StageProgress المرتبط بالـ Stage
            var stageProgress = await _unitOfWork.StageProgressRepository.GetByIdAsync(dto.StageProgressId)
                                 ?? throw new KeyNotFoundException("Stage progress not found");

            // 2. التأكد أن نوع الستيج هو Task
            var stage = await _unitOfWork.StageRepository.GetByIdAsync(stageProgress.StageId);
            if (stage.Type != StageType.Task)
                throw new InvalidOperationException("Stage is not of type Task");

            // 3. جلب الـ LevelProgress و الـ Enrollment للوصول لـ ApplicantId
            var levelProgress = await _unitOfWork.LevelProgressRepository.GetByIdAsync(stageProgress.LevelProgressId)
                                ?? throw new InvalidOperationException("Level progress not found");

            var enrollment = await _unitOfWork.EnrollmentRepository.GetByIdAsync(levelProgress.EnrollmentId)
                            ?? throw new InvalidOperationException("Enrollment not found");

            var applicantId = enrollment.ApplicantId;

            // 4. جلب الـ TaskPool المرتبط بالستيج
            var pool = await _unitOfWork.TasksPoolRepository.GetByStageIdAsync(stage.Id)
                       ?? throw new InvalidOperationException("No task pool found for this stage");

            // 5. اختيار تاسك عشوائي من الـ TaskApp
            var taskList = await _unitOfWork.AppTaskRepository.GetByTaskPoolIdAsync(pool.Id);
            if (taskList == null || !taskList.Any())
                throw new InvalidOperationException("No tasks available in pool");

            var selectedTask = taskList.OrderBy(t => Guid.NewGuid()).First();

            // 6. إنشاء سجل TaskApplicant
            var taskApplicant = new TaskApplicant
            {
                StageProgressId = dto.StageProgressId,
                TaskId = selectedTask.Id,
                ApplicantId = applicantId,
                AssignedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(pool.DaysToSubmit)
            };

            await _unitOfWork.TaskApplicantRepository.AddAsync(taskApplicant);
            await _unitOfWork.SaveChangesAsync();

            // 7. إرجاع DTO
            return new TaskApplicantDTO
            {
                Id = taskApplicant.Id,
                TaskId = selectedTask.Id,
                StageProgressId = dto.StageProgressId,
                AssignedDate = taskApplicant.AssignedDate,
                DueDate = taskApplicant.DueDate
            };
        }

    }

}
