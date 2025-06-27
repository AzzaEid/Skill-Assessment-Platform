using SkillAssessmentPlatform.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class CreateStageDTO
    {
        public int Id { get; set; } 
        public string? Name { get; set; }
        public string? Description { get; set; }
        public StageType? Type { get; set; }  //eg: task, inter, exam
        public int? Order { get; set; }
        public int? PassingScore { get; set; }
        public int? NoOfattempts { get; set; } = 3;
    }

}
