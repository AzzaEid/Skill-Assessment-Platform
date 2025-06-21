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
            var stageProgress = await _unitOfWork.StageProgressRepository.GetByIdAsync(dto.StageProgressId)
                                     ?? throw new KeyNotFoundException("Stage progress not found");

            var stage = await _unitOfWork.StageRepository.GetByIdAsync(stageProgress.StageId);
            if (stage.Type != StageType.Task)
                throw new InvalidOperationException("Stage is not of type Task");

            var pool = await _unitOfWork.TasksPoolRepository.GetByStageIdAsync(stage.Id)
                       ?? throw new InvalidOperationException("No task pool found for this stage");

            var taskList = await _unitOfWork.AppTaskRepository.GetByTaskPoolIdAsync(pool.Id);
            if (taskList == null || !taskList.Any())
                throw new InvalidOperationException("No tasks available in pool");

            var selectedTask = taskList.OrderBy(t => Guid.NewGuid()).First();

            var levelProgress = await _unitOfWork.LevelProgressRepository.GetByIdAsync(stageProgress.LevelProgressId)
                               ?? throw new InvalidOperationException("LevelProgress not found");

            var enrollment = await _unitOfWork.EnrollmentRepository.GetByIdAsync(levelProgress.EnrollmentId)
                             ?? throw new InvalidOperationException("Enrollment not found");

            var taskApplicant = new TaskApplicant
            {
                //StageProgressId = dto.StageProgressId,
                TaskId = selectedTask.Id,
                ApplicantId = enrollment.ApplicantId,
                StageProgressId = stageProgress.Id,
                AssignedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(pool.DaysToSubmit)
            };

            await _unitOfWork.TaskApplicantRepository.AddAsync(taskApplicant);
            await _unitOfWork.SaveChangesAsync();

            return new TaskApplicantDTO
            {
                Id = taskApplicant.Id,
                TaskId = taskApplicant.TaskId,
                ApplicantId = taskApplicant.ApplicantId,
                AssignedDate = taskApplicant.AssignedDate,
                DueDate = taskApplicant.DueDate
            };
        }


    }

}
