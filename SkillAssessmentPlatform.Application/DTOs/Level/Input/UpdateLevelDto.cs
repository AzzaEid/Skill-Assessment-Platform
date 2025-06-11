using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class UpdateLevelDto
    {
        public string Name { get; set; }
      //  public string StageName { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
    }

}
