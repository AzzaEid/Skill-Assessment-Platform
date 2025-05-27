using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class CreateEvaluationCriteriaDto
    {
        public int StageId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Weight { get; set; }
    }
}
