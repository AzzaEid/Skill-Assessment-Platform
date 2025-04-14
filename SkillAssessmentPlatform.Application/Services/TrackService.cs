using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;
using SkillAssessmentPlatform.Core.Interfaces;
using SkillAssessmentPlatform.Infrastructure.ExternalServices;

namespace SkillAssessmentPlatform.Application.Services
{
    public class TrackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public TrackService(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
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
                Levels = track.Levels.Select(level => new LevelDto
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
                }).ToList()
            };
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
            string imagePath = "default-track.png";

            if (trackDto.ImageFile != null && trackDto.ImageFile.Length > 0)
                imagePath = await _fileService.UploadFileAsync(trackDto.ImageFile, "track-images");

            var track = new Track
            {
                SeniorExaminerID = trackDto.SeniorExaminerID,
                Name = trackDto.Name,
                Description = trackDto.Description,
                Objectives = trackDto.Objectives,
                AssociatedSkills = trackDto.AssociatedSkills,
                IsActive = trackDto.IsActive,
                Image = imagePath,
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

            track.Name = trackDto.Name;
            track.Description = trackDto.Description;
            track.Objectives = trackDto.Objectives;
            track.AssociatedSkills = trackDto.AssociatedSkills;
            track.SeniorExaminerID = trackDto.SeniorExaminerID;
            track.IsActive = trackDto.IsActive;

            if (trackDto.ImageFile != null && trackDto.ImageFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(track.Image) && track.Image != "default-track.png")
                    await _fileService.DeleteFileAsync(track.Image);

                var newImagePath = await _fileService.UploadFileAsync(trackDto.ImageFile, "track-images");
                track.Image = newImagePath;
            }

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

        public async Task<CreateLevelDTO> CreateLevelAsync(int trackId, [FromBody] CreateLevelDTO dto)
        {
            var track = await _unitOfWork.TrackRepository.GetByIdAsync(trackId);
            var level = new Level
            {
                TrackId = trackId,
                Name = dto.Name,
                Description = dto.Description,
                Order = dto.Order,
                IsActive = dto.IsActive
            };

            await _unitOfWork.LevelRepository.AddAsync(level);
            await _unitOfWork.SaveChangesAsync();

            dto.Id = level.Id;
            return dto;
        }
    }
}
