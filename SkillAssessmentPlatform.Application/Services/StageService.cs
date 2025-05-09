﻿using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;
using SkillAssessmentPlatform.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.Services
{
    public class StageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<StageDetailDTO> GetStageByIdAsync(int id)
        {
            var stage = await _unitOfWork.StageRepository.GetByIdAsync(id);
            

            return new StageDetailDTO
            {
                Id = stage.Id,
                Name = stage.Name,
                Description = stage.Description,
                Type = stage.Type,
                Order = stage.Order,
                IsActive = stage.IsActive,
                PassingScore = stage.PassingScore,
               /* EvaluationCriteria = stage.EvaluationCriteria?.Select(e => new EvaluationCriteriaDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    Weight = e.Weight
                }).ToList()*/
            };
        }

        public async Task<List<EvaluationCriteriaDTO>> GetCriteriaByStageIdAsync(int stageId)
        {
            var stage = await _unitOfWork.StageRepository
                .GetByIdWithCriteriaAsync(stageId);

            if (stage == null || stage.EvaluationCriteria == null)
                return null;

            return stage.EvaluationCriteria.Select(c => new EvaluationCriteriaDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Weight = c.Weight
            }).ToList();
        }


        public async Task<EvaluationCriteriaDTO> AddCriterionAsync(int stageId, CreateEvaluationCriteriaDTO dto)
        {
            var stage = await _unitOfWork.StageRepository.GetByIdAsync(stageId);
            if (stage == null)
                return null;

            var criterion = new EvaluationCriteria
            {
                StageId = stageId,
                Name = dto.Name,
                Description = dto.Description,
                Weight = dto.Weight
            };

            await _unitOfWork.EvaluationCriteriaRepository.AddAsync(criterion);
            await _unitOfWork.SaveChangesAsync();

            return new EvaluationCriteriaDTO
            {
                Id = criterion.Id,
                Name = criterion.Name,
                Description = criterion.Description,
                Weight = criterion.Weight
            };
        }

        public async Task<bool> UpdateStageAsync(int stageId, UpdateStageDTO dto)
        {
            var stage = await _unitOfWork.StageRepository.GetByIdAsync(stageId);
            if (stage == null)
                return false;

            stage.Name = dto.Name;
            stage.Description = dto.Description;
            stage.Type = dto.Type;
            stage.Order = dto.Order;
            stage.PassingScore = dto.PassingScore;
            stage.IsActive = dto.IsActive;

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteStageAsync(int stageId)
        {
            var stage = await _unitOfWork.StageRepository.GetByIdWithCriteriaAsync(stageId);
            if (stage == null)
                return false;

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    stage.IsActive = false;

                    if (stage.EvaluationCriteria != null)
                    {
                        foreach (var criteria in stage.EvaluationCriteria)
                        {
                            criteria.IsActive = false;
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new Exception("Failed to soft-delete stage", ex);
                }
            }
        }


        public async Task<string> RestoreStageAsync(int stageId)
        {
            var stage = await _unitOfWork.StageRepository.GetByIdWithCriteriaAsync(stageId);
            if (stage == null)
                return "Stage not found";

            if (stage.IsActive)
                return "Stage is already active";

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    stage.IsActive = true;

                    if (stage.EvaluationCriteria != null)
                    {
                        foreach (var criteria in stage.EvaluationCriteria)
                        {
                            criteria.IsActive = true;
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return "Stage restored successfully with its criteria";
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new Exception("Failed to restore stage", ex);
                }
            }
        }


    }

}
