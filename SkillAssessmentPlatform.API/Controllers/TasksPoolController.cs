using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Services;

namespace SkillAssessmentPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksPoolController : ControllerBase
    {
        private readonly TasksPoolService _service;
        private readonly IResponseHandler _responseHandler;

        public TasksPoolController(TasksPoolService service, IResponseHandler responseHandler)
        {
            _service = service;
            _responseHandler = responseHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTasksPoolDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return _responseHandler.Created(result, "TasksPool created successfully");
        }

        [HttpGet("{stageId}")]
        public async Task<IActionResult> GetByStageId(int stageId)
        {
            var result = await _service.GetByStageIdAsync(stageId);
            return result != null
                ? _responseHandler.Success(result)
                : _responseHandler.NotFound("TasksPool not found");
        }

        [HttpPut]
        public async Task<IActionResult> Update(TasksPoolDto dto)
        {
            var result = await _service.UpdateAsync(dto);
            return result != null
                ? _responseHandler.Success(result, "TasksPool updated successfully")
                : _responseHandler.NotFound("TasksPool not found");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            return result
                ? _responseHandler.Success(message: "TasksPool deleted successfully")

                : _responseHandler.NotFound("TasksPool not found or already deleted");
        }
    }
}
