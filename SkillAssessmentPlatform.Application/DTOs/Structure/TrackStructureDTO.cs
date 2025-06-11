using SkillAssessmentPlatform.Application.DTOs.Structure;

namespace SkillAssessmentPlatform.Application.DTOs;
public class TrackStructureDTO
{
    public int TrackId { get; set; }
    public string? TrackName { get; set; }
    public List<LevelStructureDTO?> Levels { get; set; }
}


