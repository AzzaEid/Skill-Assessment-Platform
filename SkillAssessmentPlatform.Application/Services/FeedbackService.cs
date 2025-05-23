using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Interfaces;

namespace SkillAssessmentPlatform.Application.Services
{
    public class FeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FeedbackService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<FeedbackDTO> CreateAsync(CreateFeedbackDTO dto)
        {
            var feedback = new Feedback
            {
                ExaminerId = dto.ExaminerId,
                Comments = dto.Comments,
                TotalScore = dto.TotalScore,
                FeedbackDate = DateTime.UtcNow,
                DetailedFeedbacks = dto.DetailedFeedbacks.Select(df => new DetailedFeedback
                {
                    CriterionId = df.EvaluationCriteriaId,
                    Comments = df.Comments,
                    Score = df.Score
                }).ToList()
            };

            // Assign to the right entity
            if (dto.TaskSubmissionId.HasValue)
            {
                var submission = await _unitOfWork.TaskSubmissionRepository.GetByIdAsync(dto.TaskSubmissionId.Value);
                submission.Feedback = feedback;
            }
            else if (dto.ExamRequestId.HasValue)
            {
                var request = await _unitOfWork.ExamRequestRepository.GetByIdAsync(dto.ExamRequestId.Value);
                request.Feedback = feedback;
            }
            else if (dto.InterviewBookId.HasValue)
            {
                var interview = await _unitOfWork.InterviewBookRepository.GetByIdAsync(dto.InterviewBookId.Value);
                interview.Feedback = feedback;
            }

            await _unitOfWork.FeedbackRepository.AddAsync(feedback);
            await _unitOfWork.SaveChangesAsync();

            return new FeedbackDTO
            {
                Id = feedback.Id,
                ExaminerId = feedback.ExaminerId,
                Comments = feedback.Comments,
                TotalScore = feedback.TotalScore,
                FeedbackDate = feedback.FeedbackDate
            };
        }

        public async Task<FeedbackDTO?> GetByIdAsync(int id)
        {
            var feedback = await _unitOfWork.FeedbackRepository.GetByIdAsync(id);
            if (feedback == null)
                return null;

            return new FeedbackDTO
            {
                Id = feedback.Id,
                ExaminerId = feedback.ExaminerId,
                Comments = feedback.Comments,
                TotalScore = feedback.TotalScore,
                FeedbackDate = feedback.FeedbackDate
            };
        }

        public async Task<IEnumerable<FeedbackDTO>> GetByExaminerIdAsync(string examinerId)
        {
            var feedbacks = await _unitOfWork.FeedbackRepository.GetByExaminerIdAsync(examinerId);

            return feedbacks.Select(f => new FeedbackDTO
            {
                Id = f.Id,
                ExaminerId = f.ExaminerId,
                Comments = f.Comments,
                TotalScore = f.TotalScore,
                FeedbackDate = f.FeedbackDate
            });
        }

    }


}
