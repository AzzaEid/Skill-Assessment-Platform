using System.Linq.Expressions;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<T> GetByIdAsync(string id);
        Task<T> GetByIdAsync(int id);
        Task<int> GetTotalCountAsync();
        IQueryable<T> GetAllQueryable();
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> GetPagedQueryable(int page, int pageSize);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(string id);
        Task<bool> DeleteAsync(int id);
        void DeleteEntity(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        void RemoveRange(IEnumerable<T> entities);

    }
}