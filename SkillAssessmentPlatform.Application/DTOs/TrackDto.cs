using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillAssessmentPlatform.Core.Entities;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class TrackDto
    {
        public int Id { get; set; }
        public string SeniorExaminerID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Objectives { get; set; }
        public string AssociatedSkills { get; set; }
        public bool IsActive { get; set; }
        public string Image { get; set; }

        // public List<Level> levels { get; set; } = new List<Level>();
        public List<LevelDto> Levels { get; set; }

    }
}