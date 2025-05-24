using Microsoft.EntityFrameworkCore.Storage;
using SkillAssessmentPlatform.Core.Interfaces.Repository;

namespace SkillAssessmentPlatform.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ITrackRepository TrackRepository { get; }
        ILevelRepository LevelRepository { get; }
        IStageRepository StageRepository { get; }
        IEvaluationCriteriaRepository EvaluationCriteriaRepository { get; }
        IExamRepository ExamRepository { get; }
        IInterviewRepository InterviewRepository { get; }
        ITasksPoolRepository TasksPoolRepository { get; }
        IAppTaskRepository AppTaskRepository { get; }
        ITaskApplicantRepository TaskApplicantRepository { get; }




        #region repos
        IGenericRepository<T> Repository<T>() where T : class;
        TRepository GetCustomRepository<TRepository>() where TRepository : class;
        IAuthRepository AuthRepository { get; }
        IUserRepository UserRepository { get; }
        IApplicantRepository ApplicantRepository { get; }
        IExaminerRepository ExaminerRepository { get; }
        ISeniorRepository SeniorRepository { get; }
        IExaminerLoadRepository ExaminerLoadRepository { get; }
        IEnrollmentRepository EnrollmentRepository { get; }
        ILevelProgressRepository LevelProgressRepository { get; }
        IStageProgressRepository StageProgressRepository { get; }
        IAssociatedSkillsRepository AssociatedSkillsRepository { get; }


        IAppointmentRepository AppointmentRepository { get; }
        IInterviewBookRepository InterviewBookRepository { get; }
        IExamRequestRepository ExamRequestRepository { get; }
        ITaskSubmissionRepository TaskSubmissionRepository { get; }

        #endregion

        #region methods
        //  AppTask<IDbContextTransaction> BeginTransactionAsync();
        Task<int> SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<int> CompleteAsync();

        #endregion
    }
}