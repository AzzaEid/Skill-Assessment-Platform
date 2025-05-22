using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Services;

namespace SkillAssessmentPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskSubmissionsController : ControllerBase
    {
        private readonly TaskSubmissionService _service;

        public TaskSubmissionsController(TaskSubmissionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitTask([FromBody] CreateTaskSubmissionDTO dto)
        {
            var result = await _service.SubmitTaskAsync(dto);
            return Ok(result);
        }
    }

}
