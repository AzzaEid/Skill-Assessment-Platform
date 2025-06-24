namespace SkillAssessmentPlatform.Application.Abstract
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key) where T : class;
        Task CreateAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class;
        Task RemoveAsync(string key);
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> getItem, TimeSpan? expiration = null) where T : class;
    }
}


