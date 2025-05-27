using AutoMapper;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Entities.TrackLevelStage;

//using SkillAssessmentPlatform.Core.Entities.TrackLevelStage.SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Interfaces;
using SkillAssessmentPlatform.Infrastructure.ExternalServices;


namespace SkillAssessmentPlatform.Application.Services
{
    public class TrackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly ILogger<TrackService> _logger;

        public TrackService(IUnitOfWork unitOfWork,
            IFileService fileService, IMapper mapper
            , ILogger<TrackService> logger)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _mapper = mapper;
            _logger = logger;
        }

        public IMapper Get_mapper()
        {
            return _mapper;
        }

        public async Task<TrackDetialDto> GetTrackByIdAsync(int id, IMapper _mapper)
        {
            var track = await _unitOfWork.TrackRepository.GetTrackWithDetailsAsync(id);
            if (track == null) return null;

            return new TrackDetialDto
            {
                Id = track.Id,
                SeniorExaminerID = track.SeniorExaminerID,
                Name = track.Name,
                Description = track.Description,
                Objectives = track.Objectives,
                AssociatedSkills = _mapper.Map<List<AssociatedSkillDTO>>(track.AssociatedSkills),
                IsActive = track.IsActive,
                Image = track.Image,
                Levels = track.Levels.Select(level => new LevelDetailDto
                {
                    Id = level.Id,
                    TrackId = level.TrackId,
                    Name = level.Name,
                    Description = level.Description,
                    Order = level.Order,
                    IsActive = level.IsActive,
                    Stages = level.Stages?.Select(stage => new StageDetailDTO
                    {
                        Id = stage.Id,
                        Name = stage.Name,
                        Description = stage.Description,
                        Type = stage.Type,
                        Order = stage.Order,
                        PassingScore = stage.PassingScore,
                        IsActive = stage.IsActive
                    }).ToList()
                }).ToList()
            };
        }
        public async Task<IEnumerable<TrackShortDto>> GetAllTracksSummaryAsync()
        {
            var traks = await _unitOfWork.TrackRepository.GetAllAsync();

            return _mapper.Map<List<TrackShortDto>>(traks);
        }

        public async Task<IEnumerable<TrackDetialDto>> GetAllTracksAsync()
        {
            var tracks = await _unitOfWork.TrackRepository.GetAllWithDetailsAsync();
            return tracks
                .Select(t => new TrackDetialDto
                {
                    Id = t.Id,
                    SeniorExaminerID = t.SeniorExaminerID,
                    Name = t.Name,
                    Description = t.Description,
                    Objectives = t.Objectives,
                    AssociatedSkills = _mapper.Map<List<AssociatedSkillDTO>>(t.AssociatedSkills),
                    IsActive = t.IsActive,
                    Image = t.Image,
                    Levels = t.Levels.Select(level => new LevelDetailDto
                    {
                        Id = level.Id,
                        TrackId = level.TrackId,
                        Name = level.Name,
                        Description = level.Description,
                        //  Order = level.Order,
                        IsActive = level.IsActive,
                        Stages = level.Stages?.Select(stage => new StageDetailDTO
                        {
                            Id = stage.Id,
                            Name = stage.Name,
                            Description = stage.Description,
                            Type = stage.Type,
                            Order = stage.Order,
                            PassingScore = stage.PassingScore,
                            IsActive = stage.IsActive
                        }).ToList()
                    }).ToList()
                });
        }

        public async Task<TrackDetialDto> GetTrackStructure(int id)
        {
            var track = await _unitOfWork.TrackRepository.GetTrackWithDetailsAsync(id);
            if (track == null) throw new KeyNotFoundException($"No thrack with id = {id} ");
            return _mapper.Map<TrackDetialDto>(track);
        }

        public async Task<IEnumerable<TrackDetialDto>> GetNotActiveTracksAsync()
        {
            var tracks = await _unitOfWork.TrackRepository.GetAllAsync();

            return tracks
                .Where(t => t.IsActive == false)
                .Select(t => new TrackDetialDto
                {
                    Id = t.Id,
                    SeniorExaminerID = t.SeniorExaminerID,
                    Name = t.Name,
                    Description = t.Description,
                    Objectives = t.Objectives,
                    AssociatedSkills = _mapper.Map<List<AssociatedSkillDTO>>(t.AssociatedSkills),
                    IsActive = t.IsActive,
                    Image = t.Image
                });
        }


        public async Task<CreateTrackDTO> CreateTrackAsync(CreateTrackDTO trackDto)
        {
            string imagePath = "default-track.png";
            if (trackDto.ImageFile != null && trackDto.ImageFile.Length > 0)
            {
                imagePath = await _fileService.UploadFileAsync(trackDto.ImageFile, "track-images");
            }

            var track = new Track
            {
                Name = trackDto.Name,
                Description = trackDto.Description,
                Objectives = trackDto.Objectives,
                Image = imagePath,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _unitOfWork.TrackRepository.AddAsync(track);
            await _unitOfWork.SaveChangesAsync(); 

            if (!string.IsNullOrEmpty(trackDto.SeniorExaminerID))
            {
                var examiner = await _unitOfWork.ExaminerRepository.GetByIdAsync(trackDto.SeniorExaminerID);
                if (examiner == null)
                    throw new Exception($"Examiner with ID {trackDto.SeniorExaminerID} not found.");

                var isAlreadySenior = await _unitOfWork.SeniorRepository.GetSeniorByTrackIdAsync(track.Id);
                if (isAlreadySenior != null)
                    throw new Exception("This examiner is already assigned as a Senior Examiner.");

                var success = await _unitOfWork.SeniorRepository.AssignSeniorToTrackAsync(examiner, track);
                if (!success)
                    throw new Exception("Failed to assign Senior Examiner.");
            }

            if (trackDto.AssociatedSkills != null && trackDto.AssociatedSkills.Any())
            {
                var associatedSkills = trackDto.AssociatedSkills.Select(skill => new AssociatedSkill
                {
                    TrackId = track.Id,
                    Name = skill.Name,
                    Description = skill.Description
                }).ToList();

                await _unitOfWork.AssociatedSkillsRepository.AddRangeAsync(associatedSkills);
                await _unitOfWork.SaveChangesAsync();
            }

            return trackDto;
        }
        public async Task<bool> CreateTrackStructureAsync(TrackStructureDTO structureDTO)
        {

            var track = await _unitOfWork.TrackRepository.GetByIdAsync(structureDTO.TrackId);
            if (track == null)
                throw new KeyNotFoundException($"Track with ID {structureDTO.TrackId} not found");

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {

                    foreach (var levelDTO in structureDTO.Levels)
                    {
                        var level = new Level
                        {
                            TrackId = (int)structureDTO.TrackId,
                            Name = levelDTO.Name,
                            Description = levelDTO.Description,
                            Order = (int)levelDTO.Order,
                            IsActive = true
                        };

                        await _unitOfWork.LevelRepository.AddAsync(level);
                        await _unitOfWork.SaveChangesAsync();


                        foreach (var stageDTO in levelDTO.Stages)
                        {
                            var stage = new Stage
                            {
                                LevelId = level.Id,
                                Name = stageDTO.Name,
                                Description = stageDTO.Description,
                                Type = stageDTO.Type,
                                Order = (int)stageDTO.Order,
                                IsActive = true,
                                PassingScore = (int)stageDTO.PassingScore
                            };

                            await _unitOfWork.StageRepository.AddAsync(stage);
                            await _unitOfWork.SaveChangesAsync();


                            // based on stageType
                            await AddStageDetailsAsync(stageDTO, stage.Id);

                            foreach (var criteriaDTO in stageDTO.EvaluationCriteria)
                            {
                                var criteria = new EvaluationCriteria
                                {
                                    StageId = stage.Id,
                                    Name = criteriaDTO.Name,
                                    Description = criteriaDTO.Description,
                                    Weight = (float)criteriaDTO.Weight
                                };

                                await _unitOfWork.EvaluationCriteriaRepository.AddAsync(criteria);
                            }
                            await _unitOfWork.SaveChangesAsync();
                        }
                    }

                    await transaction.CommitAsync();
                    return true;
                }
                /* catch (Exception ex)
                 {
                     await transaction.RollbackAsync();
                     //_logger.LogError(ex, "Error creating track structure for track ID {TrackId}", structureDTO.TrackId);
                     throw new Exception("Failed to create track structure, ", ex);
                 } */
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Failed to create track structure: {ex.Message} | Inner: {ex.InnerException?.Message}", ex);
                }

            }
        }
        private async Task AddStageDetailsAsync(StageStructureDTO stageDTO, int stageId)
        {
            switch (stageDTO.Type)
            {
                case StageType.Exam:
                    // Convert List<string> to enum flags
                    QuestionType combinedTypes = QuestionType.None;
                    foreach (var type in stageDTO.Exam.QuestionsType)
                    {
                        if (Enum.TryParse(type, true, out QuestionType parsed))
                        {
                            combinedTypes |= parsed;
                        }
                    }
                    if (stageDTO.Exam != null)
                    {
                        var exam = new Exam
                        {
                            StageId = stageId,
                            DurationMinutes = stageDTO.Exam.DurationMinutes,
                            Difficulty = stageDTO.Exam.Difficulty,
                            QuestionsType = combinedTypes
                        };

                        await _unitOfWork.ExamRepository.AddAsync(exam);
                        await _unitOfWork.SaveChangesAsync();
                    }
                    break;

                case StageType.Interview:
                    if (stageDTO.Interview != null)
                    {
                        var interview = new Interview
                        {
                            StageId = stageId,
                            MaxDaysToBook = stageDTO.Interview.MaxDaysToBook,
                            DurationMinutes = stageDTO.Interview.DurationMinutes,
                            Instructions = stageDTO.Interview.Instructions,
                        };

                        await _unitOfWork.InterviewRepository.AddAsync(interview);
                        await _unitOfWork.SaveChangesAsync();
                    }
                    break;

                case StageType.Task:
                    if (stageDTO.TasksPool != null)
                    {
                        var task = new TasksPool
                        {
                            StageId = stageId,
                            DaysToSubmit = stageDTO.TasksPool.DaysToSubmit,
                            Description = stageDTO.TasksPool.Description,
                            Requirements = stageDTO.TasksPool.Requirements,

                        };

                        await _unitOfWork.TasksPoolRepository.AddAsync(task);
                        await _unitOfWork.SaveChangesAsync();
                    }
                    break;
            }
        }

        public async Task<CreateTrackDTO> UpdateTrackAsync(CreateTrackDTO trackDto, int trackId)
        {
            var existingTrack = await _unitOfWork.TrackRepository.GetByIdAsync(trackId);
            if (existingTrack == null)
                throw new Exception($"Track with ID {trackId} not found.");

            // تحديث الصورة إذا تم رفع صورة جديدة
            if (trackDto.ImageFile != null && trackDto.ImageFile.Length > 0)
            {
                existingTrack.Image = await _fileService.UploadFileAsync(trackDto.ImageFile, "track-images");
            }

            // تحديث البيانات الأساسية
            existingTrack.Name = trackDto.Name;
            existingTrack.Description = trackDto.Description;
            existingTrack.Objectives = trackDto.Objectives;
            existingTrack.SeniorExaminerID = trackDto.SeniorExaminerID;

            // تحديث Senior Examiner إذا تم تغييره
            if (!string.IsNullOrEmpty(trackDto.SeniorExaminerID))
            {
                var seniorExaminer = await _unitOfWork.ExaminerRepository.GetByIdAsync(trackDto.SeniorExaminerID);
                if (seniorExaminer == null)
                    throw new Exception($"Senior Examiner with ID {trackDto.SeniorExaminerID} not found.");
                existingTrack.SeniorExaminer = seniorExaminer;
            }

            // حذف المهارات القديمة
            /*
            var existingSkills = await _unitOfWork.AssociatedSkillsRepository.GetByTrackIdAsync(trackId);
            if (existingSkills.Any())
            {
                _unitOfWork.AssociatedSkillRepository.RemoveRange(existingSkills);
            }/*/

            // إضافة المهارات الجديدة
            if (trackDto.AssociatedSkills != null && trackDto.AssociatedSkills.Any())
            {
                var newAssociatedSkills = trackDto.AssociatedSkills.Select(skill => new AssociatedSkill
                {
                    TrackId = trackId,
                    Name = skill.Name,
                    Description = skill.Description,
                }).ToList();

                await _unitOfWork.AssociatedSkillsRepository.AddRangeAsync(newAssociatedSkills);
            }

            //_unitOfWork.TrackRepository.Update(existingTrack); لازم التراك يورث من الجنرك
            await _unitOfWork.SaveChangesAsync();

            return trackDto;
        }



        public async Task<bool> DeActivateTrackAsync(int id)
        {
            var track = await _unitOfWork.TrackRepository.GetTrackWithDetailsAsync(id);
            if (track == null) return false;

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    track.IsActive = false;

                    if (track.Levels != null)
                    {
                        foreach (var level in track.Levels)
                        {
                            level.IsActive = false;

                            if (level.Stages != null)
                            {
                                foreach (var stage in level.Stages)
                                {
                                    stage.IsActive = false;

                                    if (stage.EvaluationCriteria != null)
                                    {
                                        foreach (var criteria in stage.EvaluationCriteria)
                                        {
                                            criteria.IsActive = false;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new Exception("Failed to deactivate track structure", ex);
                }
            }
        }


        public async Task<bool> ActivateTrackAsync(int id)
        {
            var track = await _unitOfWork.TrackRepository.GetByIdAsync(id);
            if (track == null) return false;
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    track.IsActive = true;
                    await _unitOfWork.SaveChangesAsync();

                    foreach (var level in track.Levels)
                    {
                        level.IsActive = true;
                        await _unitOfWork.SaveChangesAsync();
                        foreach (var satge in level.Stages)
                        {
                            satge.IsActive = true;
                            await _unitOfWork.SaveChangesAsync();
                            foreach (var cirteria in satge.EvaluationCriteria)
                            {
                                cirteria.IsActive = true;
                                await _unitOfWork.SaveChangesAsync();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _unitOfWork.RollbackTransactionAsync();
                    throw new Exception("Failed to delete track structure, ", ex);
                }
            }
            return true;
        }

        public async Task<IEnumerable<TrackDetialDto>> GetOnlyActiveTracksAsync()
        {
            var tracks = await _unitOfWork.TrackRepository.GetOnlyActiveTracksAsync();

            return tracks.Select(t => new TrackDetialDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Objectives = t.Objectives,
                Image = t.Image,
                IsActive = t.IsActive,
                AssociatedSkills = _mapper.Map<List<AssociatedSkillDTO>>(t.AssociatedSkills),
                SeniorExaminerID = t.SeniorExaminerID
            });
        }

        public async Task<IEnumerable<TrackDetialDto>> GetOnlyDeactivatedTracksAsync()
        {
            var tracks = await _unitOfWork.TrackRepository.GetOnlyDeactivatedTracksAsync();

            return tracks.Select(t => new TrackDetialDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Objectives = t.Objectives,
                Image = t.Image,
                IsActive = t.IsActive,
                AssociatedSkills = _mapper.Map<List<AssociatedSkillDTO>>(t.AssociatedSkills),
                SeniorExaminerID = t.SeniorExaminerID
            });
        }

        public async Task<string> RestoreTrackAsync(int id)
        {
            var track = await _unitOfWork.TrackRepository.GetTrackWithDetailsAsync(id);
            if (track == null)
                return "Track not found";

            if (track.IsActive)
                return "Track is already active";

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    track.IsActive = true;

                    if (track.Levels != null)
                    {
                        foreach (var level in track.Levels)
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
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return "Track restored successfully with all related data";
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new Exception("Failed to restore track", ex);
                }
            }
        }


        public async Task<IEnumerable<ExaminerDTO>> GetWorkingExaminersByTrackIdAsync(int trackId)
        {
            var examiners = await _unitOfWork.ExaminerRepository.GetWorkingExaminersByTrackIdAsync(trackId);

            return examiners.Select(e => new ExaminerDTO
            {
                Id = e.Id,
                FullName = e.FullName,
                Email = e.Email,
                Image = e.Image,
                Specialization = e.Specialization,
                UserType = e.UserType,
                ExaminerLoads = e.ExaminerLoads?.Select(load => new ExaminerLoadDTO
                {
                    ID = load.ID.ToString(),
                    Type = load.Type.ToString(),
                    CurrWorkLoad = load.CurrWorkLoad,
                    MaxWorkLoad = load.MaxWorkLoad
                })
            });
        }
        public async Task<IEnumerable<ExaminerListDTO>> GetTrackWorkingExaminersAsync(int trackId)
        {
            var examiners = await _unitOfWork.ExaminerRepository.GetWorkingExaminersByTrackIdAsync(trackId);

            return _mapper.Map<IEnumerable<ExaminerListDTO>>(examiners);
        }

        public async Task<IEnumerable<TrackShortDto>> GetActiveTrackListAsync()
        {
            var tracks = await _unitOfWork.TrackRepository.GetOnlyActiveTracksAsync();

            return tracks.Select(t => new TrackShortDto
            {
                Id = t.Id,
                Name = t.Name,
                Image = t.Image
            });
        }

        public async Task<bool> AddLevelToTrackAsync(int trackId, CreateLevelDTO dto)
        {
            var level = new Level
            {
                Name = dto.Name,
                Description = dto.Description,
                Order = (int)dto.Order,
                IsActive = true
            };

            return await _unitOfWork.TrackRepository.AddLevelToTrackAsync(trackId, level);
        }

        public async Task<IEnumerable<StageDetailDTO>> GetTaskStagesByTrackIdAsync(int trackId)
        {
            var track = await _unitOfWork.TrackRepository.GetTrackWithDetailsAsync(trackId);
            if (track == null) throw new KeyNotFoundException("Track not found");

            var taskStages = track.Levels
                .SelectMany(l => l.Stages)
                .Where(s => s.Type == StageType.Task && s.IsActive)
                .Select(stage => new StageDetailDTO
                {
                    Id = stage.Id,
                    Name = stage.Name,
                    Description = stage.Description,
                    Type = stage.Type,
                    Order = stage.Order,
                    PassingScore = stage.PassingScore,
                    IsActive = stage.IsActive
                });

            return taskStages;
        }

    }
}