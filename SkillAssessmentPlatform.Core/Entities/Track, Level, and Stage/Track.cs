﻿using SkillAssessmentPlatform.Core.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Entities
{
    public class Track
    {
        public int Id { get; set; }  
        public int SeniorExaminerID { get; set; }  
        public string Name { get; set; }
        public string Description { get; set; }
        public string Objectives { get; set; }
        public string AssociatedSkills { get; set; }
        public bool IsActive { get; set; }
        public string Image { get; set; }  
        public DateTime CreatedAt { get; set; }

        public Examiner SeniorExaminer { get; set; }

        public ICollection<Examiner> Examiners { get; set; }

    }
}
