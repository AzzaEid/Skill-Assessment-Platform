﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class UpdateAppTaskDto : CreateAppTaskDto
    {
        public int Id { get; set; }
    }
}
