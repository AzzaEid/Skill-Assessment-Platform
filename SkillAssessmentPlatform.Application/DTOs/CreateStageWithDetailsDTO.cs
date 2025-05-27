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
      //  public int NoOfAttempts { get; set; } = 3;
        public CreateExamDetailsDto? Exam { get; set; }
        public CreateInterviewDetailsDto? Interview { get; set; }
        public CreateTasksPoolDetailsDto? TasksPool { get; set; }

        public List<EvaluationCriteriaDTO> EvaluationCriteria { get; set; }
    }


}
