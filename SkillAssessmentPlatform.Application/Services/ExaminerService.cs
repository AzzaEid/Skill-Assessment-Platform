using AutoMapper;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Common;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces;

namespace SkillAssessmentPlatform.Application.Services
{
    public class ExaminerService
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly ITrackRepository _trackRepository;
        //private readonly IWorkloadRepository _workloadRepository;
        private readonly IMapper _mapper;

        public ExaminerService(
                    IUnitOfWork unitOfWork,
                    IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<ExaminerDTO>> GetAllExaminersAsync(int page = 1, int pageSize = 10)
        {
            var examiners = await _unitOfWork.ExaminerRepository.GetPagedAsync(page, pageSize);
            var totalCount = await _unitOfWork.ExaminerRepository.GetTotalCountAsync();

            return new PagedResponse<ExaminerDTO>(
                _mapper.Map<List<ExaminerDTO>>(examiners),
                page,
                pageSize,
                totalCount
            );
        }

        public async Task<ExaminerDTO> GetExaminerByIdAsync(string id)
        {
            var examiner = await _unitOfWork.ExaminerRepository.GetByIdAsync(id);
            return _mapper.Map<ExaminerDTO>(examiner);
        }

        /*cuz-error
        public async AppTask<ExaminerDTO> UpdateExaminerAsync(string id, UpdateExaminerDTO examinerDto)
        {
            var examiner = await _unitOfWork.ExaminerRepository.GetByIdAsync(id);

           // examiner.Specialization = examinerDto.Specialization;
            //examiner.Bio = examinerDto.Bio;
            var mapped = _mapper.Map<Examiner>(examinerDto);
            var updatedExaminer = await _unitOfWork.ExaminerRepository.UpdateAsync(mapped);
            return _mapper.Map<ExaminerDTO>(updatedExaminer);
        }
        */
        public async Task<ExaminerDTO> UpdateExaminerAsync(string id, UpdateExaminerDTO examinerDto)
        {
            var examiner = await _unitOfWork.ExaminerRepository.GetByIdAsync(id);

            if (examiner == null)
                throw new UserNotFoundException("Examiner not found");

            // هنا نحدث الخصائص الموجودة فقط في dto
            _mapper.Map(examinerDto, examiner); // يتم التحديث داخل الكائن نفسه عشان ما نعمل مابنغ يدوي

            var result = await _unitOfWork.ExaminerRepository.UpdateAsync(examiner);
            if (result == null)
                throw new BadRequestException("Update failed");

            return _mapper.Map<ExaminerDTO>(result);
        }

        public async Task<List<TrackDto>> GetExaminerTracksAsync(string examinerId)
        {
            var tracks = await _unitOfWork.TrackRepository.GetByExaminerIdAsync(examinerId);

            return _mapper.Map<List<TrackDto>>(tracks);

        }
        public async Task<bool> AddTrackToExaminerAsync(string examinerId, int trackId)
        {
            var examiner = await _unitOfWork.ExaminerRepository.GetByIdAsync(examinerId);
            var track = await _unitOfWork.TrackRepository.GetByIdAsync(trackId);

            examiner.WorkingTracks.Add(track);
            await _unitOfWork.ExaminerRepository.UpdateAsync(examiner);

            return true;
        }

        public async Task<IEnumerable<ExaminerLoadDTO>> GetExaminerWorkloadAsync(string examinerId)
        {
            var workload = await _unitOfWork.ExaminerLoadRepository.GetByExaminerIdAsync(examinerId);
            return _mapper.Map<IEnumerable<ExaminerLoadDTO>>(workload);
        }

        public async Task<bool> RemoveTrackFromExaminerAsync(string examinerId, int trackId)
        {
            var examiner = await _unitOfWork.ExaminerRepository.GetByIdAsync(examinerId);
            var track = examiner.WorkingTracks.FirstOrDefault(t => t.Id == trackId);

            if (track != null)
            {
                examiner.WorkingTracks.Remove(track);
                await _unitOfWork.ExaminerRepository.UpdateAsync(examiner);
            }

            return true;
        }
    }

}
