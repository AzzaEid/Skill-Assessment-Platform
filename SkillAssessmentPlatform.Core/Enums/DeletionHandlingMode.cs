namespace SkillAssessmentPlatform.Core.Enums
{
    public enum DeletionHandlingMode
    {
        DistributeWeight = 0,        // توزيع الوزن على الباقي (frontend side)
        ReplaceWithNewCriteria = 1,  // حذف + إضافة Criterion جديدة
        UpdateAllManually = 2,       // تعديل يدوي لكل التقييمات (إعادة التوزيع يدوي)
        AddExtraWithoutChange = 3    // إضافة تقييم جديد بوزن مختلف دون تعديل البقية
    }
}
