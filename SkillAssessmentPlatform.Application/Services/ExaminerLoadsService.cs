using AutoMapper;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces;

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
            var load = await _unitOfWork.ExaminerLoadRepository.UpdateWorkLoadAsync(id, updateDto.MaxWorkLoad);
            return _mapper.Map<ExaminerLoadDTO>(load);
        }


        public async Task<IEnumerable<ExaminerLoadDTO>> CreateExaminerLoadAsync(CreateExaminerLoadListDTO createListDto)
        {

            var examiner = await _unitOfWork.ExaminerRepository.GetByIdAsync(createListDto.ExaminerID);
            if (examiner == null)
                throw new KeyNotFoundException($"Examiner with id {createListDto.ExaminerID} not found");

            var result = new List<ExaminerLoadDTO>();

            foreach (var loadDto in createListDto.examinerLoads)
            {
                var load = new ExaminerLoad
                {
                    ExaminerID = createListDto.ExaminerID,
                    Type = loadDto.Type,
                    MaxWorkLoad = loadDto.MaxWorkLoad
                };

                await _unitOfWork.ExaminerLoadRepository.AddAsync(load);
                result.Add(_mapper.Map<ExaminerLoadDTO>(load));
            }

            await _unitOfWork.SaveChangesAsync();
            return result;
        }


        public async Task DeleteLoad(int id)
        {
            var load = await _unitOfWork.ExaminerLoadRepository.GetByIdAsync(id);
            if (load == null)
                throw new KeyNotFoundException($"no examiner load with id : {id}");
            await _unitOfWork.ExaminerLoadRepository.DeleteAsync(id);
        }

    }
}
