using Microsoft.AspNetCore.Http;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Entities.TrackLevelStage;
using SkillAssessmentPlatform.Core.Entities.TrackLevelStage.SkillAssessmentPlatform.Core.Entities;

public class CreateTrackDTO
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Objectives { get; set; }
    public IFormFile? ImageFile { get; set; }
    public List<AssociatedSkill> AssociatedSkills { get; set; }
    public string? SeniorExaminerID { get; set; }


}