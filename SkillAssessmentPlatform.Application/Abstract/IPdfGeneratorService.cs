namespace SkillAssessmentPlatform.Application.Abstract
{
    public interface IPdfGeneratorService
    {
        byte[] GeneratePdfFromHtml(string htmlContent);
    }

}
