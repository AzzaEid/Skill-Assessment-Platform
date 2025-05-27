using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Services;

namespace SkillAssessmentPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppTasksController : ControllerBase
    {
        private readonly AppTaskService _service;
        private readonly IResponseHandler _responseHandler;

        public AppTasksController(AppTaskService service, IResponseHandler responseHandler)
        {
            _service = service;
            _responseHandler = responseHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAppTaskDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return _responseHandler.Created(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetBylId(int Id)
        {
            var result = await _service.GetByIdAsync(Id);
            return _responseHandler.Success(result);
        }
        [HttpGet("by-pool/{taskPoolId}")]
        public async Task<IActionResult> GetByPoolId(int taskPoolId)
        {
            var result = await _service.GetByTaskPoolIdAsync(taskPoolId);
            return _responseHandler.Success(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateAppTaskDto dto)
        {
            var result = await _service.UpdateAsync(dto);
            return result != null
                ? _responseHandler.Success(result, "Updated Successfully")
                : _responseHandler.NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? _responseHandler.Deleted() : _responseHandler.NotFound();
        }
    }
}
