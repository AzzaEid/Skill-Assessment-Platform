namespace SkillAssessmentPlatform.Application.CachedServices
{
    public static partial class CacheKeys
    {
        public const string AVAILABLE_SLOTS = "slots:examiner:{0}:start:{1}:end:{2}";
        public static readonly TimeSpan SLOTS_CACHE_DURATION = TimeSpan.FromMinutes(5);


    }
}
