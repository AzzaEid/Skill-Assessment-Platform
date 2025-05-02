using SkillAssessmentPlatform.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class StageDetailDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public StageType Type { get; set; } // or use Enum
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public double PassingScore { get; set; }
        public int NoOfattempts { get; set; } 
        public List<EvaluationCriteriaDTO> EvaluationCriteria { get; set; }
    }

}