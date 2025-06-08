using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces;

namespace SkillAssessmentPlatform.Application.Services
{

    public class EvaluationCriteriaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EvaluationCriteriaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateAsync(CreateEvaluationCriteriaDto dto)
        {
            var entity = new EvaluationCriteria
            {
                StageId = dto.StageId,
                Name = dto.Name,
                Description = dto.Description,
                Weight = dto.Weight,
                IsActive = true
            };

            await _unitOfWork.EvaluationCriteriaRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<EvaluationCriteria>> GetByStageIdAsync(int stageId)
        {
            return await _unitOfWork.EvaluationCriteriaRepository.GetByStageIdAsync(stageId);
        }
        public async Task<bool> UpdateAsync(UpdateEvaluationCriteriaDto dto)
        {
            var existing = await _unitOfWork.EvaluationCriteriaRepository.GetByIdAsync(dto.Id);
            if (existing == null || !existing.IsActive)
                return false;

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.Weight = dto.Weight;

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var entity = await _unitOfWork.EvaluationCriteriaRepository.GetByIdAsync(id);
            if (entity == null || !entity.IsActive)
                return false;

            entity.IsActive = false;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<EvaluationCriteria> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.EvaluationCriteriaRepository.GetByIdAsync(id);
            return item != null && item.IsActive ? item : null;
        }

        public async Task<IEnumerable<EvaluationCriteria>> GetAllAsync()
        {
            var all = await _unitOfWork.EvaluationCriteriaRepository.GetAllAsync();
            return all.Where(e => e.IsActive);
        }


        public async Task<bool> UpdateStageCriteriaAsync(UpdateStageCriteriaPayloadDto payload)
        {
            var stageId = payload.StageId;
            var allCurrent = (await _unitOfWork.EvaluationCriteriaRepository.GetActiveByStageIdAsync(stageId)).ToList();

            if (!allCurrent.Any())
                throw new Exception("No active criteria found for this stage.");

            if (payload.DeletionMode == DeletionHandlingMode.AddExtraWithoutChange)
                payload.DeletionMode = DeletionHandlingMode.UpdateAllManually;

            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                // Handle delete + replace
                if (payload.CriteriaIdToDelete.HasValue &&
                    payload.DeletionMode == DeletionHandlingMode.ReplaceWithNewCriteria &&
                    payload.NewCriteriaToAdd?.Any() == true)
                {
                    var toDelete = allCurrent.FirstOrDefault(c => c.Id == payload.CriteriaIdToDelete);
                    if (toDelete != null)
                        toDelete.IsActive = false;

                    foreach (var dto in payload.NewCriteriaToAdd)
                    {
                        var newCrit = new EvaluationCriteria
                        {
                            Name = dto.Name,
                            Description = dto.Description,
                            Weight = dto.Weight,
                            StageId = stageId,
                            IsActive = true
                        };
                        await _unitOfWork.EvaluationCriteriaRepository.AddAsync(newCrit);
                    }
                }
                // Handle full manual update
                else if (payload.DeletionMode == DeletionHandlingMode.UpdateAllManually)
                {
                    foreach (var crit in allCurrent)
                        crit.IsActive = false;

                    foreach (var updated in payload.UpdatedCriteria)
                    {
                        var entity = new EvaluationCriteria
                        {
                            StageId = stageId,
                            Name = updated.Name,
                            Description = updated.Description,
                            Weight = updated.Weight,
                            IsActive = true
                        };
                        await _unitOfWork.EvaluationCriteriaRepository.AddAsync(entity);
                    }

                    if (payload.NewCriteriaToAdd?.Any() == true)
                    {
                        foreach (var dto in payload.NewCriteriaToAdd)
                        {
                            var newCrit = new EvaluationCriteria
                            {
                                Name = dto.Name,
                                Description = dto.Description,
                                Weight = dto.Weight,
                                StageId = stageId,
                                IsActive = true
                            };
                            await _unitOfWork.EvaluationCriteriaRepository.AddAsync(newCrit);
                        }
                    }
                }
                // Soft delete only
                else if (payload.CriteriaIdToDelete.HasValue && payload.DeletionMode == DeletionHandlingMode.DistributeWeight)
                {
                    var toDelete = allCurrent.FirstOrDefault(c => c.Id == payload.CriteriaIdToDelete);
                    if (toDelete == null)
                        throw new BadRequestException("Criterion to delete not found.");

                    toDelete.IsActive = false;
                    float deletedWeight = toDelete.Weight;

                    float redistributeFactor = 100f / (100f - deletedWeight);
                    var saveLast = 0;
                    foreach (var c in allCurrent)
                    {
                        if (c.IsActive)
                        {
                            c.Weight = c.Weight * redistributeFactor;
                            saveLast = c.Id;
                        }

                        await _unitOfWork.EvaluationCriteriaRepository.UpdateAsync(c);
                    }


                    await _unitOfWork.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Unsupported operation mode.");
                }

                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

    }



}
