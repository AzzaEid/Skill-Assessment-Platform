using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs.plan
{
    public class TrackPlanDTO
    {
        public int Id {  get; set; }
        public List<LevelPlanDTO> levels { get; set; }
    }
}
