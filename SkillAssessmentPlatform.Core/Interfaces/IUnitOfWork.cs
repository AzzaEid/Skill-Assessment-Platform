using Microsoft.EntityFrameworkCore.Storage;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ITrackRepository TrackRepository { get; }
        ILevelRepository LevelRepository { get; }
        IStageRepository StageRepository { get; }
        IEvaluationCriteriaRepository EvaluationCriteriaRepository { get; }
        
        #region repos
        IGenericRepository<T> Repository<T>() where T : class;
        TRepository GetCustomRepository<TRepository>() where TRepository : class;
        IAuthRepository AuthRepository { get; }
        IUserRepository UserRepository { get; }
        IApplicantRepository ApplicantRepository { get; }
        IExaminerRepository ExaminerRepository { get; }
         IExaminerLoadRepository ExaminerLoadRepository { get; }
        IEnrollmentRepository EnrollmentRepository { get; }
        ILevelProgressRepository LevelProgressRepository { get; }
        IStageProgressRepository StageProgressRepository { get; }
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