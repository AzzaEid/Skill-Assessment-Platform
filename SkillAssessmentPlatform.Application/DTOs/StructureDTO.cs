

using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs;
public class TrackStructureDTO
{
    public int TrackId { get; set; }
    public string? TrackName { get; set; }
    public List<LevelStructureDTO> Levels { get; set; }
}
public class LevelStructureDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public List<StageStructureDTO> Stages { get; set; }
}

public class StageStructureDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public StageType Type { get; set; }
    public int Order { get; set; }
    public int PassingScore { get; set; }
    public int NoOfAttempts { get; set; }
    public List<EvaluationStructureCriteriaDTO> EvaluationCriteria { get; set; }
}

public class EvaluationStructureCriteriaDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public float Weight { get; set; }
}