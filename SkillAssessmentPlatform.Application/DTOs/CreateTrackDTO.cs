using Microsoft.AspNetCore.Http;
using SkillAssessmentPlatform.Application.DTOs;

public class CreateTrackDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Objectives { get; set; }
    public IFormFile? ImageFile { get; set; }
    public string? SeniorExaminerID { get; set; }
    public List<CreateAssociatedSkillDTO> AssociatedSkills { get; set; } = new List<CreateAssociatedSkillDTO>();
}


}

