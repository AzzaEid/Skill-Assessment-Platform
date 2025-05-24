using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Entities.TrackLevelStage
{
    namespace SkillAssessmentPlatform.Core.Entities
    {
        public class AssociatedSkill
        {
            public int Id { get; set; }

            public int TrackId { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            // Navigation
            public Track Track { get; set; }
        }
    }



}
