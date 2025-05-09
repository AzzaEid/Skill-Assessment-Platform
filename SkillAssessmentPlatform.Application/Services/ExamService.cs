using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Interfaces;
using SkillAssessmentPlatform.Infrastructure.ExternalServices;

namespace SkillAssessmentPlatform.Application.Services
{
    public class ExamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public ExamService(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<ExamDto> CreateExamAsync(CreateExamDto dto)
        {
            var existingExam = await _unitOfWork.ExamRepository.GetByStageIdAsync(dto.StageId);
            if (existingExam != null)
                throw new InvalidOperationException("This stage already has an exam.");

            var exam = new Exam
            {
                StageId = dto.StageId,
                DurationMinutes = dto.DurationMinutes,
                Difficulty = dto.Difficulty,
                QuestionsType = dto.QuestionsType
            };

            await _unitOfWork.ExamRepository.AddAsync(exam);
            await _unitOfWork.SaveChangesAsync();

            return new ExamDto
            {
                Id = exam.Id,
                StageId = exam.StageId,
                DurationMinutes = exam.DurationMinutes,
                Difficulty = exam.Difficulty,
                QuestionsType = exam.QuestionsType
            };
        }

        public async Task<ExamDto> GetByStageIdAsync(int stageId)
        {
            var exam = await _unitOfWork.ExamRepository.GetByStageIdAsync(stageId);
            if (exam == null || !exam.IsActive) return null;

            return new ExamDto
            {
                Id = exam.Id,
                StageId = exam.StageId,
                DurationMinutes = exam.DurationMinutes,
                Difficulty = exam.Difficulty,
                QuestionsType = exam.QuestionsType
            };
        }


        public async Task<ExamDto> UpdateExamAsync(UpdateExamDto dto)
        {
            var exam = await _unitOfWork.ExamRepository.GetByIdAsync(dto.Id);
            if (exam == null) return null;

            exam.DurationMinutes = dto.DurationMinutes;
            exam.Difficulty = dto.Difficulty;
            exam.QuestionsType = dto.QuestionsType;

            await _unitOfWork.SaveChangesAsync();

            return new ExamDto
            {
                Id = exam.Id,
                StageId = exam.StageId,
                DurationMinutes = exam.DurationMinutes,
                Difficulty = exam.Difficulty,
                QuestionsType = exam.QuestionsType
            };
        }

        public async Task<bool> SoftDeleteExamAsync(int id)
        {
            var exam = await _unitOfWork.ExamRepository.GetByIdAsync(id);
            if (exam == null || !exam.IsActive)
                return false;

            exam.IsActive = false;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }




    }
}
