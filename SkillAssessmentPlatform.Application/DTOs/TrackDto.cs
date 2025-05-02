using Microsoft.AspNetCore.Http;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class TrackDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Objectives { get; set; }
        public IFormFile? ImageFile { get; set; }
        public Dictionary<string, string> AssociatedSkills { get; set; } = new();
        public string? SeniorExaminerID { get; set; }

    }
}
