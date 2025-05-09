﻿namespace SkillAssessmentPlatform.Application.DTOs
{
    public class TrackDetialDto
    {
        public int Id { get; set; }
        public string SeniorExaminerID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Objectives { get; set; }
        public Dictionary<string, string> AssociatedSkills { get; set; } = new();

        public bool IsActive { get; set; }
        public string Image { get; set; }

        // public List<Level> levels { get; set; } = new List<Level>();
        public List<LevelDetailDto> Levels { get; set; }

    }
}