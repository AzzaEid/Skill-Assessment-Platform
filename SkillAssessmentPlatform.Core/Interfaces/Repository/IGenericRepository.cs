﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(string id);
        Task<T> GetByIdAsync(int id);
        Task<int> GetCount();
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetPagedAsync(int page, int pageSize);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(string id);
        Task<bool> DeleteAsync(int id);
        Task<int> GetTotalCountAsync();
    }
}
