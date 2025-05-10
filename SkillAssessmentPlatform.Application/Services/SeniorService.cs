using AutoMapper;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Interfaces;

namespace SkillAssessmentPlatform.Application.Services
{
    public class SeniorService
    {
        //private readonly ISeniorRepository _seniorRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SeniorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ExaminerDTO>> GetAllSeniorsAsync()
        {
            var seniors = await _unitOfWork.SeniorRepository.GetSeniorListAsync();
            return _mapper.Map<List<ExaminerDTO>>(seniors);
        }

        public async Task<ExaminerDTO?> GetSeniorByTrackIdAsync(int trackId)
        {
            var senior = await _unitOfWork.SeniorRepository.GetSeniorByTrackIdAsync(trackId);
            return _mapper.Map<ExaminerDTO>(senior);
        }

        public async Task<bool> AssignSeniorAsync(string examinerId, int trackId)
        {
            var examiner = await _unitOfWork.ExaminerRepository.GetByIdAsync(examinerId);
            if (examiner == null) return false;
            var track = await _unitOfWork.TrackRepository.GetByIdAsync(trackId);
            if (track == null) return false;
            return await _unitOfWork.SeniorRepository.AssignSeniorToTrackAsync(examiner, track);
        }

        public async Task<bool> UpdateSeniorAsync(string newExaminerId, int trackId)
        {
            var newExaminer = await _unitOfWork.ExaminerRepository.GetByIdAsync(newExaminerId);
            if (newExaminer == null) return false;
            var track = await _unitOfWork.TrackRepository.GetByIdAsync(trackId);
            if (track == null) return false;
            return await _unitOfWork.SeniorRepository.ChangeTrackSeniorAsync(newExaminer, track);
        }

        public async Task<bool> RemoveSeniorAsync(int trackId)
        {
            var track = await _unitOfWork.TrackRepository.GetByIdAsync(trackId);
            if (track == null) return false;
            return await _unitOfWork.SeniorRepository.RemoveSeniorFromTrackAsync(track);
        }
    }

}
