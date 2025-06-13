using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Interfaces;

namespace SkillAssessmentPlatform.Application.Services
{
    public class AppTaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CreationAssignmentService _creationAssignmentService;

        public AppTaskService(IUnitOfWork unitOfWork, CreationAssignmentService creationAssignmentService)
        {
            _unitOfWork = unitOfWork;
            _creationAssignmentService = creationAssignmentService;
        }

        public async Task<AppTaskDto> CreateAsync(string examinerId, CreateAppTaskDto dto)
        {
            var tasksPool = await _unitOfWork.TasksPoolRepository.GetByIdAsync(dto.TaskPoolId);
            if (tasksPool == null) throw new Exception("no related taskPool Id");
            var task = new AppTask
            {
                TaskPoolId = dto.TaskPoolId,
                Title = dto.Title,
                Description = dto.Description,
                Requirements = dto.Requirements,
                Difficulty = dto.Difficulty
            };

            await _unitOfWork.AppTaskRepository.AddAsync(task);
            await _unitOfWork.SaveChangesAsync();

            await _creationAssignmentService.UpdateTaskCreationProgressAsync(dto.TaskPoolId, examinerId);

            return new AppTaskDto
            {
                Id = task.Id,
                TaskPoolId = task.TaskPoolId,
                Title = task.Title,
                Description = task.Description,
                Requirements = task.Requirements,
                Difficulty = task.Difficulty
            };
        }

        public async Task<IEnumerable<AppTaskDto>> GetByTaskPoolIdAsync(int taskPoolId)
        {
            var tasks = await _unitOfWork.AppTaskRepository.GetByTaskPoolIdAsync(taskPoolId);
            return tasks.Select(t => new AppTaskDto
            {
                Id = t.Id,
                TaskPoolId = t.TaskPoolId,
                Title = t.Title,
                Description = t.Description,
                Requirements = t.Requirements,
                Difficulty = t.Difficulty
            });
        }
        public async Task<AppTaskDto> GetByIdAsync(int taskId)
        {
            var task = await _unitOfWork.AppTaskRepository.GetByIdAsync(taskId);
            if (task == null)
                throw new KeyNotFoundException($"AppTask with id {taskId} not found");
            return new AppTaskDto
            {
                Id = task.Id,
                TaskPoolId = task.TaskPoolId,
                Title = task.Title,
                Description = task.Description,
                Requirements = task.Requirements,
                Difficulty = task.Difficulty
            };
        }

        public async Task<AppTaskDto> UpdateAsync(UpdateAppTaskDto dto)
        {
            var task = await _unitOfWork.AppTaskRepository.GetByIdAsync(dto.Id);
            if (task == null) return null;

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Requirements = dto.Requirements;
            task.Difficulty = dto.Difficulty;

            await _unitOfWork.SaveChangesAsync();

            return new AppTaskDto
            {
                Id = task.Id,
                TaskPoolId = task.TaskPoolId,
                Title = task.Title,
                Description = task.Description,
                Requirements = task.Requirements,
                Difficulty = task.Difficulty
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _unitOfWork.AppTaskRepository.GetByIdAsync(id);
            if (task == null) return false;

            _unitOfWork.AppTaskRepository.DeleteEntity(task);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }

}
