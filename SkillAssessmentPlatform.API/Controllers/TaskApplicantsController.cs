using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Services;

namespace SkillAssessmentPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskApplicantsController : ControllerBase
    {
        private readonly TaskApplicantService _service;

        public TaskApplicantsController(TaskApplicantService service)
        {
            _service = service;
        }

        [HttpPost("assign-random")]
        public async Task<IActionResult> AssignRandomTask([FromBody] AssignTaskDTO dto)
        {
            var result = await _service.AssignRandomTaskAsync(dto);
            return Ok(result);
        }
    }

}
