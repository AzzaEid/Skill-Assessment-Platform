using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Interfaces;

namespace SkillAssessmentPlatform.Application.Services
{
    public class DetailedFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetailedFeedbackService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DetailedFeedbackDTO>> GetByFeedbackIdAsync(int feedbackId)
        {
            var list = await _unitOfWork.DetailedFeedbackRepository.GetByFeedbackIdAsync(feedbackId);
            return list.Select(df => new DetailedFeedbackDTO
            {
                Id = df.Id,
                FeedbackId = df.FeedbackId,
                EvaluationCriteriaId = df.CriterionId,
                CriterionName = df.EvaluationCriteria.Name,
                Comments = df.Comments,
                Score = df.Score
            });
        }
    }

}
