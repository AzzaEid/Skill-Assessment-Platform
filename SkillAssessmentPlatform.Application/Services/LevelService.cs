﻿using Microsoft.AspNetCore.Mvc;
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

        public LevelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<List<LevelDetailDto>> GetLevelsByLevelIdAsync(int trackId)
        {
            var levels = await _unitOfWork.LevelRepository.GetLevelsByIdAsync(trackId);
            if (levels == null || !levels.Any()) return new List<LevelDetailDto>();

            return levels.Select(level => new LevelDetailDto
            {
                Id = level.Id,
                TrackId = level.TrackId,
                Name = level.Name,
                Description = level.Description,
                Order = level.Order,
                IsActive = level.IsActive,
                Stages = level.Stages?.Select(stage => new StageDetailDTO
                {
                    Name = stage.Name,
                    Description = stage.Description,
                    Type = stage.Type,
                    Order = stage.Order,
                    PassingScore = stage.PassingScore
                }).ToList()
            }).ToList();
        }



        /*public async AppTask<List<LevelDetailDto>> GetLevelsByTrackIdAsync(int id)
        {
            var levels = await _unitOfWork.LevelRepository.GetLevelsByTrackIdAsync(id);
            if (levels == null || !levels.Any()) return new List<LevelDetailDto>();

            return levels.Select(level => new LevelDetailDto
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

        public async Task<List<StageDetailDTO>> GetStagesByLevelIdAsync(int levelId)
        {
            var level = await _unitOfWork.LevelRepository.GetLevelWithStagesAsync(levelId);
            if (level == null || level.Stages == null)
                return null;

            return level.Stages.Select(stage => new StageDetailDTO
            {
                Id = stage.Id,

                Name = stage.Name,
                Description = stage.Description,
                Type = stage.Type,
                Order = stage.Order,
                PassingScore = stage.PassingScore
            }).ToList();
        }

        [HttpPost("{levelId}/stages")]
        public async Task<CreateStageDTO> CreateStage(int levelId, [FromBody] CreateStageDTO dto)
        {

            var level = await _unitOfWork.LevelRepository.GetByIdAsync(levelId);

            var stage = new Stage
            {
                LevelId = levelId,
                Name = dto.Name,
                Description = dto.Description,
                Type = (Core.Enums.StageType)dto.Type,
                Order = (int)dto.Order,
                PassingScore = (int)dto.PassingScore,
                IsActive = true
            };

            await _unitOfWork.StageRepository.AddAsync(stage);
            await _unitOfWork.SaveChangesAsync();

            dto.Id = stage.Id;
            return dto;
        }

        public async Task<bool> DeleteLevelAsync(int id)
        {
            var level = await _unitOfWork.LevelRepository.GetLevelWithStagesAsync(id);
            if (level == null) return false;

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    level.IsActive = false;

                    if (level.Stages != null)
                    {
                        foreach (var stage in level.Stages)
                        {
                            stage.IsActive = false;
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new Exception("Failed to soft-delete level", ex);
                }
            }
        }

        public async Task<string> RestoreLevelAsync(int levelId)
        {
            var level = await _unitOfWork.LevelRepository.GetLevelWithStagesAsync(levelId);
            if (level == null)
                return "Level not found";

            if (level.IsActive)
                return "Level is already active";

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    level.IsActive = true;

                    if (level.Stages != null)
                    {
                        foreach (var stage in level.Stages)
                        {
                            stage.IsActive = true;

                            if (stage.EvaluationCriteria != null)
                            {
                                foreach (var criteria in stage.EvaluationCriteria)
                                {
                                    criteria.IsActive = true;
                                }
                            }
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return "Level restored successfully with its stages and criteria";
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new Exception("Failed to restore level", ex);
                }
            }
        }




    }
}
