namespace SkillAssessmentPlatform.Application.CachedServices
{
    public static partial class CacheKeys
    {
        #region cache keys 
        public const string ACTIVE_TRACKS_KEY = "active_tracks";
        public const string ALL_TRACKS_SUMMARY_KEY = "all_tracks_summary";
        public const string TRACK_STRUCTURE_KEY = "track_structure_{0}";
        #endregion

        #region cache expire time
        public static readonly TimeSpan TRACKS_CACHE_DURATION = TimeSpan.FromHours(2);
        public static readonly TimeSpan TRACK_STRUCTURE_CACHE_DURATION = TimeSpan.FromMinutes(30);
        #endregion

    }
}
