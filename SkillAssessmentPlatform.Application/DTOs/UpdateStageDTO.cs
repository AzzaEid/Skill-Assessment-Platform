﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class UpdateStageDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }  // "Task", "Exam", "Interview"
        public int Order { get; set; }
        public int PassingScore { get; set; }
        public bool IsActive { get; set; }
    }

}
