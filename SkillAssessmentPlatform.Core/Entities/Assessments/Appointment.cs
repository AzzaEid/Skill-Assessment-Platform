﻿using SkillAssessmentPlatform.Core.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews
{
    public class Appointment
    {
        public int Id { get; set; }
        public string ExaminerId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsBooked { get; set; }
        // Navigation properties
        public Examiner Examiner { get; set; }
        public ICollection<InterviewBook> InterviewBooks { get; set; } = new List<InterviewBook>();
    }
}
