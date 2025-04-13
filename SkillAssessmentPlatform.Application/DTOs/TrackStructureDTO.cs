using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class TrackStructureDTO
    {
        public int TrackId { get; set; }
        public List<LevelCreateDTO> Levels { get; set; }
    }

    public class LevelCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public List<StageCreateDTO> Stages { get; set; }
    }

    public class StageCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int Order { get; set; }
        public int PassingScore { get; set; }
        public List<CriteriaCreateDTO> EvaluationCriteria { get; set; }
    }

    public class CriteriaCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Weight { get; set; }
        public int TrackId { get; set; }
        public string TrackName { get; set; }
     //   public List<LevelDTO> Levels { get; set; }
    }
}