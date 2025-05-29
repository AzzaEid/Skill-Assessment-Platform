using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.DTOs.CreateAssignment;
using SkillAssessmentPlatform.Core.Entities.Management;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Interfaces;

namespace SkillAssessmentPlatform.Application.Services
{
    public class CreationAssignmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly NotificationService _notificationService;

        public CreationAssignmentService(IUnitOfWork unitOfWork,
                                         IMapper mapper,
                                         NotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
        }
        public async Task<IEnumerable<CreationAssignmentDTO>> GetAll()
        {
            var results = await _unitOfWork.CreationAssignmentRepository.GetOverdueAssignmentsAsync();
            return _mapper.Map<IEnumerable<CreationAssignmentDTO>>(results);
        }
        public async Task<IEnumerable<CreationAssignmentDTO>> GetBySeniorId(string seniorId)
        {
            var assignments = await _unitOfWork.CreationAssignmentRepository
                 .GetPendingBySeniorAsync(seniorId);

            return _mapper.Map<IEnumerable<CreationAssignmentDTO>>(assignments);
        }

        // إنشاء تكليف جديد (يستخدمه Senior Examiner)
        public async Task<CreationAssignmentDTO> CreateAssignmentAsync(CreateAssignmentDTO createDto)
        {
            var assignment = new CreationAssignment
            {
                ExaminerId = createDto.ExaminerId,
                StageId = createDto.StageId,
                Type = createDto.Type,
                Status = AssignmentStatus.Assigned,
                AssignedDate = DateTime.Now,
                DueDate = createDto.DueDate,
                AssignedBySeniorId = createDto.AssignedBySeniorId,
                Notes = createDto.Notes
            };

            await _unitOfWork.CreationAssignmentRepository.AddAsync(assignment);
            await _unitOfWork.SaveChangesAsync();

            var dto = _mapper.Map<CreationAssignmentDTO>(assignment);
            if (createDto.Type == CreationType.Exam)
            {
                var relatedExam = await _unitOfWork.ExamRepository.GetByStageIdAsync(createDto.StageId);
                dto.Exam = _mapper.Map<ExamDto>(relatedExam);
            }
            else
            {
                var relatedTask = await _unitOfWork.TasksPoolRepository.GetByStageIdAsync(createDto.StageId);
                dto.TasksPool = _mapper.Map<TasksPoolDto>(relatedTask);
            }
            return dto;
        }

        // جلب التكليفات للمختبر
        public async Task<IEnumerable<CreationAssignmentDTO>> GetExaminerAssignmentsAsync(string examinerId)
        {
            var assignments = await _unitOfWork.CreationAssignmentRepository
                .GetPendingByExaminerIdAsync(examinerId);

            return _mapper.Map<IEnumerable<CreationAssignmentDTO>>(assignments);
        }

        public async Task UpdateTaskCreationProgressAsync(int taskId)
        {
            var task = await _unitOfWork.AppTaskRepository.GetByIdAsync(taskId);
            var tasksPool = await _unitOfWork.TasksPoolRepository.GetByIdAsync(task.TaskPoolId);

            var assignment = _unitOfWork.CreationAssignmentRepository
                .GetAllQueryable()
                .Where(ca => ca.StageId == tasksPool.StageId &&
                            ca.Type == CreationType.Task &&
                            ca.Status != AssignmentStatus.Completed)
                .Include(a => a.Examiner)
                .Include(a => a.Stage)
                .FirstOrDefault();

            if (assignment == null)
                throw new Exception("This Task id not related to any assignments!");
            //نغير حالة الاسسمنت - نزيد الكرنت وورك لود
            await _unitOfWork.CreationAssignmentRepository.UpdateStatusAsync(assignment.Id, AssignmentStatus.Completed);

            await _unitOfWork.ExaminerLoadRepository.DecrementWorkloadAsync(assignment.ExaminerId, LoadType.TaskCreation);

            // نخبر السينيور
            await _notificationService.SendNotificationAsync(
                assignment.AssignedBySeniorId,
                NotificationType.TaskCreationCompleted,
                $"Examiner {assignment.Examiner.FullName} create task for staeg {assignment.Stage.Name}");

        }
        public async Task UpdateExamCreationAssignmentStatusAsync(int assignmentId)
        {

            var creation = await _unitOfWork.CreationAssignmentRepository.UpdateStatusAsync(assignmentId, AssignmentStatus.Completed);

            await _unitOfWork.ExaminerLoadRepository.DecrementWorkloadAsync(creation.ExaminerId, LoadType.ExamCreation);
        }
        public async Task CancelCreationAssignmentStatusAsync(int assignmentId)
        {

            var creation = await _unitOfWork.CreationAssignmentRepository.UpdateStatusAsync(assignmentId, AssignmentStatus.Cancelled);

            await _unitOfWork.ExaminerLoadRepository.DecrementWorkloadAsync(creation.ExaminerId, LoadType.ExamCreation);
        }
    }
}
