using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class CreateExamDetailsDto
    {
        public int DurationMinutes { get; set; }
        public string Difficulty { get; set; }
        public QuestionType QuestionsType { get; set; }
    }
}
