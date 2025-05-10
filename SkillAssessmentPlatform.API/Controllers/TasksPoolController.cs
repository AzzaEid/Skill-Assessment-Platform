using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Services;

namespace SkillAssessmentPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksPoolController : ControllerBase
    {
        private readonly TasksPoolService _service;

        public TasksPoolController(TasksPoolService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTasksPoolDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Created("", result);
        }

        [HttpGet("{stageId}")]
        public async Task<IActionResult> GetByStageId(int stageId)
        {
            var result = await _service.GetByStageIdAsync(stageId);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(TasksPoolDto dto)
        {
            var result = await _service.UpdateAsync(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }

}
