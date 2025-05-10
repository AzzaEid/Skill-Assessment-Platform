using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Interfaces;

namespace SkillAssessmentPlatform.Application.Services
{
    public class TasksPoolService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TasksPoolService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TasksPoolDto> CreateAsync(CreateTasksPoolDto dto)
        {
            var entity = new TasksPool
            {
                StageId = dto.StageId,
                DaysToSubmit = dto.DaysToSubmit,
                Description = dto.Description,
                Requirements = dto.Requirements
            };

            await _unitOfWork.TasksPoolRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return new TasksPoolDto
            {
                Id = entity.Id,
                StageId = entity.StageId,
                DaysToSubmit = entity.DaysToSubmit,
                Description = entity.Description,
                Requirements = entity.Requirements
            };
        }

        public async Task<TasksPoolDto> GetByStageIdAsync(int stageId)
        {
            var entity = await _unitOfWork.TasksPoolRepository.GetByStageIdAsync(stageId);
            if (entity == null) return null;

            return new TasksPoolDto
            {
                Id = entity.Id,
                StageId = entity.StageId,
                DaysToSubmit = entity.DaysToSubmit,
                Description = entity.Description,
                Requirements = entity.Requirements
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.TasksPoolRepository.GetByIdAsync(id);
            if (entity == null) return false;

            _unitOfWork.TasksPoolRepository.DeleteEntity(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<TasksPoolDto> UpdateAsync(TasksPoolDto dto)
        {
            var entity = await _unitOfWork.TasksPoolRepository.GetByIdAsync(dto.Id);
            if (entity == null) return null;

            entity.DaysToSubmit = dto.DaysToSubmit;
            entity.Description = dto.Description;
            entity.Requirements = dto.Requirements;

            await _unitOfWork.SaveChangesAsync();
            return dto;
        }
    }

}
