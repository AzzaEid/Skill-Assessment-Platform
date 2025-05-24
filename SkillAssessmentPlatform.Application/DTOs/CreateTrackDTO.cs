using Microsoft.AspNetCore.Http;
using SkillAssessmentPlatform.Application.DTOs;

using System.Text.Json;

public class CreateTrackDTO
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Objectives { get; set; }
    public IFormFile? ImageFile { get; set; }

    // جديد - لاستقبال JSON كـ string من FormData
    public string? AssociatedSkillsJsonString { get; set; }

    // محسوبة - لتحويل JSON string إلى قائمة
    public List<CreateAssociatedSkillDTO>? AssociatedSkillsJson
    {
        get
        {
            if (string.IsNullOrEmpty(AssociatedSkillsJsonString))
                return new List<CreateAssociatedSkillDTO>();

            try
            {
                return JsonSerializer.Deserialize<List<CreateAssociatedSkillDTO>>(AssociatedSkillsJsonString)
                       ?? new List<CreateAssociatedSkillDTO>();
            }
            catch
            {
                return new List<CreateAssociatedSkillDTO>();
            }
        }
    }

    public string? SeniorExaminerID { get; set; }
}
