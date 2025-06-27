using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs.Auth.Inputs
{
    public class ResetPasswordDTO
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
