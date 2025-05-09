﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Interfaces;

namespace SkillAssessmentPlatform.Application.Services
{
    public class AppTaskService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppTaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AppTaskDto> CreateAsync(CreateAppTaskDto dto)
        {
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
