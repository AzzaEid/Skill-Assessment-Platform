using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs.Auth
{
    public class ExaminerRegisterDTO : UserRegisterDTO
    {
        public int WorkingTrackId { get; set; }
    }
}
