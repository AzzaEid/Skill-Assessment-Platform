using Microsoft.EntityFrameworkCore.Storage;
using SkillAssessmentPlatform.Core.Interfaces;
using SkillAssessmentPlatform.Core.Interfaces.Repository;

namespace SkillAssessmentPlatform.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction;
        private bool _disposed = false;

        // Repositories injected via DI
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly IApplicantRepository _applicantRepository;
        private readonly IExaminerRepository _examinerRepository;
        private readonly IExaminerLoadRepository _examinerLoadRepository;
        private readonly ITrackRepository _trackRepository;
        private readonly ILevelRepository _levelRepository;
        private readonly IStageRepository _stageRepository;
        private readonly IEvaluationCriteriaRepository _evaluationCriteriaRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly ILevelProgressRepository _levelProgressRepository;
        private readonly IStageProgressRepository _stageProgressRepository;
        private readonly ISeniorRepository _seniorRepository;
        private readonly IExamRepository _examRepository;
        private readonly IInterviewRepository _interviewRepository;
        private readonly ITasksPoolRepository _tasksPoolRepository;
        private readonly IAppTaskRepository _appTaskRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public UnitOfWork(
            AppDbContext context,
            IAuthRepository authRepository,
            IUserRepository userRepository,
            IApplicantRepository applicantRepository,
            IExaminerRepository examinerRepository,
            ITrackRepository trackRepository,
            ILevelRepository levelRepository,
            IStageRepository stageRepository,
            IEvaluationCriteriaRepository evaluationCriteriaRepository,
            IExaminerLoadRepository examinerLoadRepository,
            IEnrollmentRepository enrollmentRepository,
            ILevelProgressRepository levelProgressRepository,
            IStageProgressRepository stageProgressRepository,
            ISeniorRepository seniorRepository,
            IExamRepository examRepository,
            IInterviewRepository interviewRepository,
            IAppTaskRepository appTaskRepository,
            ITasksPoolRepository tasksPoolRepository,
            IAppointmentRepository appointmentRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _authRepository = authRepository;
            _userRepository = userRepository;
            _applicantRepository = applicantRepository;
            _examinerRepository = examinerRepository;
            _examinerLoadRepository = examinerLoadRepository;
            _trackRepository = trackRepository;
            _levelRepository = levelRepository;
            _stageRepository = stageRepository;
            _evaluationCriteriaRepository = evaluationCriteriaRepository;
            _enrollmentRepository = enrollmentRepository;
            _levelProgressRepository = levelProgressRepository;
            _stageProgressRepository = stageProgressRepository;
            _seniorRepository = seniorRepository;

            _examRepository = examRepository;
            _interviewRepository = interviewRepository;
            _tasksPoolRepository = tasksPoolRepository;
            _appTaskRepository = appTaskRepository;
            _appointmentRepository = appointmentRepository;
        }

        // Custom Repositories exposed
        public IAuthRepository AuthRepository => _authRepository;
        public IUserRepository UserRepository => _userRepository;
        public IApplicantRepository ApplicantRepository => _applicantRepository;
        public IExaminerRepository ExaminerRepository => _examinerRepository;
        public ISeniorRepository SeniorRepository => _seniorRepository;
        public IExaminerLoadRepository ExaminerLoadRepository => _examinerLoadRepository;
        public ITrackRepository TrackRepository => _trackRepository;
        public ILevelRepository LevelRepository => _levelRepository;
        public IStageRepository StageRepository => _stageRepository;
        public IEvaluationCriteriaRepository EvaluationCriteriaRepository => _evaluationCriteriaRepository;
        public IEnrollmentRepository EnrollmentRepository => _enrollmentRepository;
        public ILevelProgressRepository LevelProgressRepository => _levelProgressRepository;
        public IStageProgressRepository StageProgressRepository => _stageProgressRepository;
        public IExamRepository ExamRepository => _examRepository;
        public IInterviewRepository InterviewRepository => _interviewRepository;
        public ITasksPoolRepository TasksPoolRepository => _tasksPoolRepository;

        public IAppTaskRepository AppTaskRepository => _appTaskRepository;

        public IAppointmentRepository AppointmentRepository => _appointmentRepository;

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_transaction != null)
                throw new InvalidOperationException("A transaction is already in progress");

            _transaction = await _context.Database.BeginTransactionAsync();
            return _transaction;
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction == null)
                throw new InvalidOperationException("No transaction to commit");

            try
            {
                await _transaction.CommitAsync();
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null)
                throw new InvalidOperationException("No transaction to rollback");

            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public TRepository GetCustomRepository<TRepository>() where TRepository : class
        {
            throw new NotImplementedException();
        }

        public Task<int> CompleteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
