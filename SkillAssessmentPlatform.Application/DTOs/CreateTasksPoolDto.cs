﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class CreateTasksPoolDto
    {
        public int StageId { get; set; }
        public int DaysToSubmit { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
    }

}
