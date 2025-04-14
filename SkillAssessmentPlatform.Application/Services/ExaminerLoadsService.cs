using AutoMapper;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.Services
{
    public class ExaminerLoadsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExaminerLoadsService(
        IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ExaminerLoadDTO>> GetByExaminerIdAsync(string examinerId)
        {
            var loads = await _unitOfWork.ExaminerLoadRepository.GetByExaminerIdAsync(examinerId);
            if (loads == null)
                throw new BadRequestException($"There's no Loads for examiner with id {examinerId}");

            return _mapper.Map<IEnumerable<ExaminerLoadDTO>>(loads);
        }

        public async Task<ExaminerLoadDTO> GetByIdAsync(int id)
        {
            var load = await _unitOfWork.ExaminerLoadRepository.GetByIdAsync(id);
            if (load == null)
                throw new KeyNotFoundException($"no examiner load with id : {id}");
            return _mapper.Map<ExaminerLoadDTO>(load);
        }

        public async Task<ExaminerLoadDTO> UpdateWorkLoadAsync(int id, UpdateWorkLoadDTO updateDto)
        {
            var load = await _unitOfWork.ExaminerLoadRepository.UpdateWorkLoadAsync(id, updateDto.WorkLoad);
            return _mapper.Map<ExaminerLoadDTO>(load);
        }

        public async Task<ExaminerLoadDTO> CreateExaminerLoadAsync(CreateExaminerLoadDTO createDto)
        {
            var load = _mapper.Map<ExaminerLoad>(createDto);

            // Check if examiner exists
            var examiner = await _unitOfWork.ExaminerRepository.GetByIdAsync(createDto.ExaminerID);
            if (examiner == null)
                throw new KeyNotFoundException($"Examiner with id {createDto.ExaminerID} not found");

            // Add the load
            await _unitOfWork.ExaminerLoadRepository.AddAsync(load);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ExaminerLoadDTO>(load);
        }
    
    }
}
