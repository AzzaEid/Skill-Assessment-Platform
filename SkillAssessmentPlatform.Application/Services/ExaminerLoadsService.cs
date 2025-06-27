using AutoMapper;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Application.DTOs.CreateAssignment;
using SkillAssessmentPlatform.Application.DTOs.Examiner.Input;
using SkillAssessmentPlatform.Application.DTOs.ExaminerDashboard;
using SkillAssessmentPlatform.Application.DTOs.StageProgress.Output;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces;

namespace SkillAssessmentPlatform.Application.Services
{
    public class ExaminerLoadsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ExaminerLoadsService> _logger;
        public ExaminerLoadsService(
        IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ExaminerLoadsService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
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
                // if (loadDto.Type == LoadType.ExamCreation || )

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


        //// ========================== Examiner Dashboard =============================/////
        public async Task<ExaminerDashboardSummaryDTO> GetDashboardSummaryAsync(string examinerId)
        {
            _logger.LogWarning($"examiner no {examinerId}");
            var examiner = await _unitOfWork.ExaminerRepository.GetByIdAsync(examinerId);
            if (examiner == null)
                throw new KeyNotFoundException($"Examiner with id {examinerId} not found");

            var summary = new ExaminerDashboardSummaryDTO();

            // Get supervised stage progresses
            var supervisedStages = await _unitOfWork.StageProgressRepository
                .GetPendingByExaminerIdAsync(examinerId);
            _logger.LogWarning($"supervisedStages no {supervisedStages.Count()}");
            foreach (var stageProgress in supervisedStages)
            {
                var stage = await _unitOfWork.StageRepository.GetByIdAsync(stageProgress.StageId);
                var applicantId = stageProgress.LevelProgress.Enrollment.ApplicantId;
                _logger.LogWarning($"==============>>> stage type {stage.Type.ToString()} - no {stage.Id} applicant {applicantId}");
                switch (stage.Type)
                {
                    case StageType.Task:
                        var taskSubmissions = await _unitOfWork.TaskSubmissionRepository
                            .GetPendingByApplicantIdAsync(applicantId);
                        summary.PendingTaskSubmissions += taskSubmissions == null ? 0 : 1;
                        break;

                    case StageType.Interview:
                        var pendingRequests = await _unitOfWork.InterviewBookRepository
                            .GetPendingByApplicantIdAsync(applicantId);
                        summary.PendingInterviewRequests += pendingRequests == null ? 0 : 1;

                        var scheduledInterviews = await _unitOfWork.InterviewBookRepository
                            .GetScheduledByApplicantIdAsync(applicantId);
                        summary.ScheduledInterviews += scheduledInterviews == null ? 0 : 1;
                        break;

                    case StageType.Exam:
                        var examReviews = await _unitOfWork.ExamRequestRepository
                            .GetCompletedPendingReviewByApplicantAsync(applicantId);
                        summary.PendingExamReviews += examReviews == null ? 0 : 1;
                        break;
                }
            }

            // Get creation tasks assignments
            var taskCreationAssignments = await _unitOfWork.CreationAssignmentRepository
                .GetPendingTasksByExaminerIdAsync(examinerId);
            summary.PendingTaskCreations = taskCreationAssignments.Count();

            var examCreationAssignments = await _unitOfWork.CreationAssignmentRepository
                .GetPendingExamsByExaminerIdAsync(examinerId);
            summary.PendingExamCreations = examCreationAssignments.Count();

            return summary;
        }

        // Detailed methods for each task type
        public async Task<IEnumerable<ExaminerTaskSubmissionDTO>> GetPendingTaskSubmissionsAsync(string examinerId)
        {
            var supervisedStages = await _unitOfWork.StageProgressRepository
                .GetByPendingExaminerIdAndTypeAsync(examinerId, StageType.Task);

            var result = new List<ExaminerTaskSubmissionDTO>();

            foreach (var stageProgress in supervisedStages)
            {
                var applicantId = stageProgress.LevelProgress.Enrollment.ApplicantId;
                var submission = await _unitOfWork.TaskSubmissionRepository
                    .GetPendingByApplicantIdAsync(applicantId);

                if (submission != null)
                {
                    var taskApplicant = await _unitOfWork.TaskApplicantRepository
                        .GetByIdAsync(submission.TaskApplicantId);
                    var task = await _unitOfWork.AppTaskRepository.GetByIdAsync(taskApplicant.TaskId);

                    var dto = new ExaminerTaskSubmissionDTO
                    {
                        Id = submission.Id,
                        StageProgressId = stageProgress.Id,
                        TaskId = task.Id,
                        TaskTitle = task.Title,
                        ApplicantId = applicantId,
                        SubmissionUrl = submission.SubmissionUrl,
                        SubmissionDate = submission.SubmissionDate,
                        DueDate = taskApplicant.DueDate,
                        DaysWaiting = (DateTime.Now - submission.SubmissionDate).Days,
                        IsLate = submission.SubmissionDate > taskApplicant.DueDate,
                        Status = submission.Status,
                        StageId = stageProgress.StageId,
                        StageName = stageProgress.Stage.Name,
                        TrackName = stageProgress.LevelProgress.Enrollment.Track.Name
                    };

                    result.Add(dto);
                }
            }

            return result.OrderBy(x => x.SubmissionDate);
        }

        public async Task<IEnumerable<ExaminerInterviewRequestDTO>> GetPendingInterviewRequestsAsync(string examinerId)
        {
            var supervisedStages = await _unitOfWork.StageProgressRepository
                .GetByPendingExaminerIdAndTypeAsync(examinerId, StageType.Interview);

            var result = new List<ExaminerInterviewRequestDTO>();
            foreach (var stageProgress in supervisedStages)
            {
                var applicantId = stageProgress.LevelProgress.Enrollment.ApplicantId;
                var interviewBook = await _unitOfWork.InterviewBookRepository
                    .GetPendingByApplicantIdAsync(applicantId);

                if (interviewBook != null)
                {
                    var interview = await _unitOfWork.InterviewRepository.GetByIdAsync(interviewBook.InterviewId);
                    var applicant = await _unitOfWork.ApplicantRepository.GetByIdAsync(applicantId);
                    var dto = new ExaminerInterviewRequestDTO
                    {
                        Id = interviewBook.Id,
                        StageProgressId = stageProgress.Id,
                        InterviewId = interview.Id,
                        ApplicantId = applicantId,
                        ApplicantName = applicant.FullName,
                        RequestDate = stageProgress.StartDate,
                        DaysWaiting = (DateTime.Now - stageProgress.StartDate).Days,
                        Status = interviewBook.Status,
                        StageId = stageProgress.StageId,
                        StageName = stageProgress.Stage.Name,
                        TrackName = stageProgress.LevelProgress.Enrollment.Track.Name,
                        MaxDaysToBook = interview.MaxDaysToBook,
                        StartDate = interviewBook.Appointment.StartTime,
                        EndDate = interviewBook.Appointment.EndTime
                    };

                    result.Add(dto);
                }
            }

            return result.OrderBy(x => x.RequestDate);
        }

        public async Task<IEnumerable<ExaminerScheduledInterviewDTO>> GetScheduledInterviewsAsync(string examinerId)
        {
            var supervisedStages = await _unitOfWork.StageProgressRepository
                .GetByPendingExaminerIdAndTypeAsync(examinerId, StageType.Interview);

            var result = new List<ExaminerScheduledInterviewDTO>();

            foreach (var stageProgress in supervisedStages)
            {
                var applicantId = stageProgress.LevelProgress.Enrollment.ApplicantId;
                var interviewBook = await _unitOfWork.InterviewBookRepository
                    .GetScheduledByApplicantIdAsync(applicantId);

                if (interviewBook != null)
                {
                    var interview = await _unitOfWork.InterviewRepository.GetByIdAsync(interviewBook.InterviewId);
                    var applicant = await _unitOfWork.ApplicantRepository.GetByIdAsync(applicantId);

                    var dto = new ExaminerScheduledInterviewDTO
                    {
                        Id = interviewBook.Id,
                        StageProgressId = stageProgress.Id,
                        ApplicantId = applicantId,
                        ApplicantName = applicant.FullName,
                        ScheduledDate = interviewBook.Appointment.StartTime,
                        MeetingLink = interviewBook.MeetingLink,
                        Status = interviewBook.Status,
                        StageId = stageProgress.StageId,
                        StageName = stageProgress.Stage.Name,
                        TrackName = stageProgress.LevelProgress.Enrollment.Track.Name,
                        DurationMinutes = interview.DurationMinutes
                    };

                    result.Add(dto);
                }
            }

            return result.OrderBy(x => x.ScheduledDate);
        }

        public async Task<IEnumerable<ExaminerExamReviewDTO>> GetPendingExamReviewsAsync(string examinerId)
        {
            var supervisedStages = await _unitOfWork.StageProgressRepository
                .GetByPendingExaminerIdAndTypeAsync(examinerId, StageType.Exam);

            var result = new List<ExaminerExamReviewDTO>();

            foreach (var stageProgress in supervisedStages)
            {

                var applicantId = stageProgress.LevelProgress.Enrollment.ApplicantId;
                var request = await _unitOfWork.ExamRequestRepository
                    .GetCompletedPendingReviewByApplicantAsync(applicantId);
                if (request != null)
                {
                    _logger.LogError($"======> nowUTC{DateTime.UtcNow} Now {DateTime.Now} -- {DateTime.UtcNow - request.ScheduledDate}");
                    var exam = await _unitOfWork.ExamRepository.GetByIdAsync(request.ExamId);
                    var applicant = await _unitOfWork.ApplicantRepository.GetByIdAsync(applicantId);
                    var dto = new ExaminerExamReviewDTO
                    {
                        Id = request.Id,
                        StageProgressId = stageProgress.Id,
                        ExamId = exam.Id,
                        ApplicantId = applicantId,
                        ApplicantName = applicant.FullName,
                        ScheduledDate = DateTime.UtcNow,
                        DaysWaiting = (DateTime.UtcNow - request.ScheduledDate).Days,
                        Status = request.Status,
                        StageId = stageProgress.StageId,
                        StageName = stageProgress.Stage.Name,
                        TrackName = stageProgress.LevelProgress.Enrollment.Track.Name,
                        Difficulty = exam.Difficulty
                    };

                    result.Add(dto);
                }
            }

            return result.OrderBy(x => x.ScheduledDate);
        }
        public async Task<IEnumerable<StageProgressDTO>> GetPendingAsync(string examinerId)
        {
            _logger.LogError($"Id============================{examinerId}");
            var progresses = await _unitOfWork.StageProgressRepository.GetByPendingExaminerIdAndTypeAsync(examinerId, StageType.Exam);
            return _mapper.Map<IEnumerable<StageProgressDTO>>(progresses);

        }

        public async Task<IEnumerable<CreationAssignmentDTO>> GetExaminerTaskAssignmentsAsync(string examinerId)
        {
            var assignments = await _unitOfWork.CreationAssignmentRepository
                .GetPendingTasksByExaminerIdAsync(examinerId);
            return _mapper.Map<IEnumerable<CreationAssignmentDTO>>(assignments);

        }
        public async Task<IEnumerable<CreationAssignmentDTO>> GetExaminerExamAssignmentsAsync(string examinerId)
        {
            var assignments = await _unitOfWork.CreationAssignmentRepository
                .GetPendingExamsByExaminerIdAsync(examinerId);

            return _mapper.Map<IEnumerable<CreationAssignmentDTO>>(assignments);
        }




    }
}
