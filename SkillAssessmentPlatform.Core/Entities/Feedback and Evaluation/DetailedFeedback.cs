﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation
{
    public class DetailedFeedback
    {

        public int Id { get; set; }
        public int FeedbackId { get; set; }
        public int CriterionId { get; set; }
        public string Comments { get; set; }
        public decimal Score { get; set; }
    }
}
