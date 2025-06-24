namespace SkillAssessmentPlatform.Application.CachedServices
{
    public static partial class CacheKeys
    {

        public const string EXAMINERS_KEY = "examiners_page_{0}_size_{1}";

        public static readonly TimeSpan EXAMINERS_CACHE_DURATION = TimeSpan.FromMinutes(15);

    }
}
