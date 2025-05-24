using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class CreateStageWithDetailsDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public StageType Type { get; set; }
        public int Order { get; set; }
        public int PassingScore { get; set; }

        public ExamDto? Exam { get; set; }
        public InterviewDto? Interview { get; set; }
        public TasksPoolDto? TasksPool { get; set; }

        public List<EvaluationCriteriaDTO> EvaluationCriteria { get; set; }
    }

}
