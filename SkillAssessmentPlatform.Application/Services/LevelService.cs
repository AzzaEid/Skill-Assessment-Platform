using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.Services
{
    public class LevelService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LevelService (IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<List<LevelDto>> GetLevelsByLevelIdAsync(int trackId)
        {
            var levels = await _unitOfWork.LevelRepository.GetLevelsByIdAsync(trackId);
            if (levels == null || !levels.Any()) return new List<LevelDto>();

            return levels.Select(level => new LevelDto
            {
                Id = level.Id,
                TrackId = level.TrackId,
                Name = level.Name,
                Description = level.Description,
                Order = level.Order,
                IsActive = level.IsActive,
                Stages = level.Stages?.Select(stage => new StageDTO
                {
                    Name = stage.Name,
                    Description = stage.Description,
                    Type = stage.Type,
                    Order = stage.Order,
                    PassingScore = stage.PassingScore
                }).ToList()
            }).ToList();
        }



        /*public async Task<List<LevelDto>> GetLevelsByTrackIdAsync(int id)
        {
            var levels = await _unitOfWork.LevelRepository.GetLevelsByTrackIdAsync(id);
            if (levels == null || !levels.Any()) return new List<LevelDto>();

            return levels.Select(level => new LevelDto
            {
                Id = level.Id,
                Name = level.Name,
                StageName = level.StageName,
                Description = level.Description,
                Order = level.Order,
                IsActive = level.IsActive,
                Stages = level.Stage.ToList()
            }).ToList();
        }*/


        public async Task<bool> UpdateLevelAsync(int id, UpdateLevelDto dto)
        {
            var level = await _unitOfWork.LevelRepository.GetByIdAsync(id);
            if (level == null) return false;
            level.Id = id;
            level.Name = dto.Name;
            level.Description = dto.Description;
            level.Order = dto.Order;
            level.IsActive = dto.IsActive;

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<StageDTO>> GetStagesByLevelIdAsync(int levelId)
        {
            var level = await _unitOfWork.LevelRepository.GetLevelWithStagesAsync(levelId);
            if (level == null || level.Stages == null)
                return null;

            return level.Stages.Select(stage => new StageDTO
            {
                Name = stage.Name,
                Description = stage.Description,
                Type = stage.Type,
                Order = stage.Order,
                PassingScore = stage.PassingScore
            }).ToList();
        }


        /* public async Task<List<StageDTO>> GetStagesByLevelIdAsync(int levelId)
         {
             var level = await _unitOfWork.LevelRepository.GetLevelWithStagesAsync(levelId);
             if (level == null || level.Stages == null)
                 return null;

             return level.Stages.Select(stage => new StageDTO
             {
                 Name = stage.Name,
                 Description = stage.Description,
                 Type = stage.Type,
                 Order = stage.Order,
                 PassingScore = stage.PassingScore
             }).ToList();
         }*/



    }
}
