

using SkillAssessmentPlatform.Core.Enums;

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
    public List<EvaluationStructureCriteriaDTO> EvaluationCriteria { get; set; }
}

public class EvaluationStructureCriteriaDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public float Weight { get; set; }
}