using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillAssessmentPlatform.Application.DTOs;


namespace SkillAssessmentPlatform.Application.DTOs
{
    public class TaskApplicantDTO
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string ApplicantId { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime DueDate { get; set; }
    }

}
