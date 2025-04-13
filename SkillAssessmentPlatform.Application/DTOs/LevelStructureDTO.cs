

using SkillAssessmentPlatform.Core.Enums;

public class LevelDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public List<StageDTO> Stages { get; set; }
}

public class StageDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public StageType Type { get; set; }
    public int Order { get; set; }
    public int PassingScore { get; set; }
    public List<EvaluationCriteriaDTO> EvaluationCriteria { get; set; }
}

public class EvaluationCriteriaDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public float Weight { get; set; }
}