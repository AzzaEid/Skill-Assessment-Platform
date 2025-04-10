using Microsoft.EntityFrameworkCore.Storage;
using SkillAssessmentPlatform.Core.Interfaces;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        private readonly ITrackRepository _trackRepository;
        private readonly ILevelRepository _levelRepository;
        private readonly IStageRepository _stageRepository;
        private readonly IEvaluationCriteriaRepository _evaluationCriteriaRepository;

        public UnitOfWork(
            AppDbContext context,
            IAuthRepository authRepository,
            IUserRepository userRepository,
            IApplicantRepository applicantRepository,
            IExaminerRepository examinerRepository,
            ITrackRepository trackRepository,
            ILevelRepository levelRepository,
            IStageRepository stageRepository,
            IEvaluationCriteriaRepository evaluationCriteriaRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _authRepository = authRepository;
            _userRepository = userRepository;
            _applicantRepository = applicantRepository;
            _examinerRepository = examinerRepository;
            _trackRepository = trackRepository;
            _levelRepository = levelRepository;
            _stageRepository = stageRepository;
            _evaluationCriteriaRepository = evaluationCriteriaRepository;
        }

        // Custom Repositories exposed
        public IAuthRepository AuthRepository => _authRepository;
        public IUserRepository UserRepository => _userRepository;
        public IApplicantRepository ApplicantRepository => _applicantRepository;
        public IExaminerRepository ExaminerRepository => _examinerRepository;

        public ITrackRepository TrackRepository => _trackRepository;
        public ILevelRepository LevelRepository => _levelRepository;
        public IStageRepository StageRepository => _stageRepository;
        public IEvaluationCriteriaRepository EvaluationCriteriaRepository => _evaluationCriteriaRepository;

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