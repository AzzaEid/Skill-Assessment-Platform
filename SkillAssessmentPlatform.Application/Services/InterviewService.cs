using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Interfaces;

namespace SkillAssessmentPlatform.Application.Services
{
    public class InterviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InterviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<InterviewDto> CreateInterviewAsync(CreateInterviewDto dto)
        {
            var interview = new Interview
            {
                StageId = dto.StageId,
                MaxDaysToBook = dto.MaxDaysToBook,
                DurationMinutes = dto.DurationMinutes,
                Instructions = dto.Instructions,
                IsActive = true,
            };

            await _unitOfWork.InterviewRepository.AddAsync(interview);
            await _unitOfWork.SaveChangesAsync();

            return new InterviewDto
            {
                Id = interview.Id,
                StageId = interview.StageId,
                MaxDaysToBook = interview.MaxDaysToBook,
                DurationMinutes = interview.DurationMinutes,
                Instructions = interview.Instructions,
            };
        }

        public async Task<InterviewDto> GetByStageIdAsync(int stageId)
        {
            var interview = await _unitOfWork.InterviewRepository.GetByStageIdAsync(stageId);
            if (interview == null || !interview.IsActive) return null;

            return new InterviewDto
            {
                Id = interview.Id,
                StageId = interview.StageId,
                MaxDaysToBook = interview.MaxDaysToBook,
                DurationMinutes = interview.DurationMinutes,
                Instructions = interview.Instructions,
            };
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var interview = await _unitOfWork.InterviewRepository.GetByIdAsync(id);
            if (interview == null || !interview.IsActive) return false;

            interview.IsActive = false;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<InterviewDto> UpdateInterviewAsync(InterviewDto dto)
        {
            var interview = await _unitOfWork.InterviewRepository.GetByIdAsync(dto.Id);
            if (interview == null) return null;

            interview.MaxDaysToBook = dto.MaxDaysToBook;
            interview.DurationMinutes = dto.DurationMinutes;
            interview.Instructions = dto.Instructions;

            await _unitOfWork.SaveChangesAsync();

            return new InterviewDto
            {
                Id = interview.Id,
                StageId = interview.StageId,
                MaxDaysToBook = interview.MaxDaysToBook,
                DurationMinutes = interview.DurationMinutes,
                Instructions = interview.Instructions,
            };
        }
    }
}
