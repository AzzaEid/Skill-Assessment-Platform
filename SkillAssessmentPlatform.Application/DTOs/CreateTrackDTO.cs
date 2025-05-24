using Microsoft.AspNetCore.Http;
using SkillAssessmentPlatform.Application.DTOs;

using System.Text.Json;

public class CreateTrackDTO
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Objectives { get; set; }
    public IFormFile? ImageFile { get; set; }

    public string? AssociatedSkillsJson { get; set; }

    // json to list
    public List<CreateAssociatedSkillDTO> GetAssociatedSkills()
    {
        return string.IsNullOrEmpty(AssociatedSkillsJson)
            ? new List<CreateAssociatedSkillDTO>()
            : JsonSerializer.Deserialize<List<CreateAssociatedSkillDTO>>(AssociatedSkillsJson)
              ?? new List<CreateAssociatedSkillDTO>();
    }


    public string? SeniorExaminerID { get; set; }
}
