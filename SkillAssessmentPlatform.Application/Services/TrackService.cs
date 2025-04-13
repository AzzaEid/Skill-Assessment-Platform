
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;
using SkillAssessmentPlatform.Core.Interfaces;
using SkillAssessmentPlatform.Infrastructure.Data;

namespace SkillAssessmentPlatform.Application.Services
{
    public class TrackService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrackService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TrackDto> GetTrackByIdAsync(int id)
        {
            var track = await _unitOfWork.TrackRepository.GetTrackWithDetailsAsync(id);
            if (track == null) return null;

            return new TrackDto
            {
                Id = track.Id,
                SeniorExaminerID = track.SeniorExaminerID,
                Name = track.Name,
                Description = track.Description,
                Objectives = track.Objectives,
                AssociatedSkills = track.AssociatedSkills,
                IsActive = track.IsActive,
                Image = track.Image,
                levels = track.Levels.ToList()
            };
        }

        public async Task<bool> CreateTrackStructureAsync(TrackStructureDTO structureDTO)
        {
            // التحقق من وجود المسار
            var track = await _unitOfWork.TrackRepository.GetByIdAsync(structureDTO.TrackId);
            if (track == null)
                throw new KeyNotFoundException($"Track with ID {structureDTO.TrackId} not found");

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    // إضافة المستويات
                    foreach (var levelDTO in structureDTO.Levels)
                    {
                        var level = new Level
                        {
                            TrackId = structureDTO.TrackId,
                            Name = levelDTO.Name,
                            Description = levelDTO.Description,
                            Order = levelDTO.Order,
                            IsActive = true
                        };

                        await _unitOfWork.LevelRepository.AddAsync(level);
                        await _unitOfWork.SaveChangesAsync();

                        // إضافة المراحل لكل مستوى
                        foreach (var stageDTO in levelDTO.Stages)
                        {
                            var stage = new Stage
                            {
                                LevelId = level.Id,
                                Name = stageDTO.Name,
                                Description = stageDTO.Description,
                                Type = stageDTO.Type,
                                Order = stageDTO.Order,
                                IsActive = true,
                                PassingScore = stageDTO.PassingScore
                            };

                       //     await _unitOfWork.StageRepository.AddAsync(stage);
                            await _unitOfWork.SaveChangesAsync();

                            // إضافة معايير التقييم لكل مرحلة
                            foreach (var criteriaDTO in stageDTO.EvaluationCriteria)
                            {
                                var criteria = new EvaluationCriteria
                                {
                                    StageId = stage.Id,
                                    Name = criteriaDTO.Name,
                                    Description = criteriaDTO.Description,
                                    Weight = criteriaDTO.Weight
                                };

                             //   await _unitOfWork.EvaluationCriteriaRepository.AddAsync(criteria);
                            }
                            await _unitOfWork.SaveChangesAsync();
                        }
                    }

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    //_logger.LogError(ex, "Error creating track structure for track ID {TrackId}", structureDTO.TrackId);
                    throw new Exception("Failed to create track structure", ex);
                }
            }
        }
        public async Task<IEnumerable<TrackDto>> GetAllTracksAsync()
        {
            var tracks = await _unitOfWork.TrackRepository.GetAllAsync();

            return tracks.Select(t => new TrackDto
            {
                Id = t.Id,
                SeniorExaminerID = t.SeniorExaminerID,
                Name = t.Name,
                Description = t.Description,
                Objectives = t.Objectives,
                AssociatedSkills = t.AssociatedSkills,
                IsActive = t.IsActive,
                Image = t.Image
            });
        }



        public async Task<CreateTrackDTO> CreateTrackAsync(CreateTrackDTO trackDto)
        {
            var track = new Track
            {
                SeniorExaminerID = trackDto.SeniorExaminerID,
                Name = trackDto.Name,
                Description = trackDto.Description,
                Objectives = trackDto.Objectives,
                AssociatedSkills = trackDto.AssociatedSkills,
                IsActive = trackDto.IsActive,
                Image = trackDto.Image,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.TrackRepository.AddAsync(track);
            await _unitOfWork.SaveChangesAsync();

            trackDto.Id = track.Id;
            return trackDto;
        }



        public async Task<CreateTrackDTO> UpdateTrackAsync(CreateTrackDTO trackDto)
        {
            var track = await _unitOfWork.TrackRepository.GetByIdAsync(trackDto.Id);
            if (track == null) return null;

            track.SeniorExaminerID = trackDto.SeniorExaminerID;
            track.Name = trackDto.Name;
            track.Description = trackDto.Description;
            track.Objectives = trackDto.Objectives;
            track.AssociatedSkills = trackDto.AssociatedSkills;
            track.IsActive = trackDto.IsActive;
            track.Image = trackDto.Image;

            await _unitOfWork.SaveChangesAsync();

            return trackDto;
        }

        public async Task<bool> DeActivateTrackAsync(int id)
        {
            var track = await _unitOfWork.TrackRepository.GetByIdAsync(id);
            if (track == null) return false;

            track.IsActive = false;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

    }
}