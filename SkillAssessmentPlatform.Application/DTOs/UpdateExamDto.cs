using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class UpdateExamDto
    {

        public int Id { get; set; } 
        public int DurationMinutes { get; set; }
        public string Difficulty { get; set; }
        public List<string> QuestionsType { get; set; }

    }
}
