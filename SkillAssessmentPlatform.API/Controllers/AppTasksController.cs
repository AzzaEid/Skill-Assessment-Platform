using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Services;

namespace SkillAssessmentPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppTasksController : ControllerBase
    {
        private readonly AppTaskService _service;

        public AppTasksController(AppTaskService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAppTaskDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpGet("by-pool/{taskPoolId}")]
        public async Task<IActionResult> GetByPoolId(int taskPoolId)
        {
            var result = await _service.GetByTaskPoolIdAsync(taskPoolId);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateAppTaskDto dto)
        {
            var result = await _service.UpdateAsync(dto);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }

}
