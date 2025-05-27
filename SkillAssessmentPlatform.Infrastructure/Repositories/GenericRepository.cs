using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;
using System.Linq.Expressions;


namespace SkillAssessmentPlatform.Infrastructure.Repositories
{

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public virtual async Task<T> AddAsync(T entity)
        {
            try
            {
                var result = await _dbSet.AddAsync(entity);
                var changes = await _context.SaveChangesAsync();

                if (changes == 0)
                {
                    throw new Exception($"Failed to create {typeof(T).Name}");
                }

                return result.Entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        public virtual async Task<T> GetByIdAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }
        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<int> GetTotalCountAsync()
        {
            return await _dbSet.CountAsync();
        }
        public virtual IQueryable<T> GetPagedQueryable(int page, int pageSize)
        {
            return _dbSet
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(string id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual void DeleteEntity(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }


    }
}