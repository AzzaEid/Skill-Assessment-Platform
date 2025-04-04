﻿using SkillAssessmentPlatform.Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        #region repos
        IGenericRepository<T> Repository<T> () where T : class;
        IAuthRepository AuthRepository { get; }
        IUserRepository UserRepository { get; }
        IApplicantRepository ApplicantRepository { get; }
        IExaminerRepository ExaminerRepository { get; }
        #endregion

        #region methods
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task<int> SaveChangesAsync();

        #endregion
    }
}
