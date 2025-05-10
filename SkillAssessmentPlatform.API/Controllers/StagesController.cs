using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Services;

namespace SkillAssessmentPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StagesController : ControllerBase
    {

        private readonly StageService _stageService;
        private readonly IResponseHandler _responseHandler;

        public StagesController(StageService stageService, IResponseHandler responseHandler)
        {
            _stageService = stageService;
            _responseHandler = responseHandler;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStageById(int id)
        {
            var stage = await _stageService.GetStageByIdAsync(id);
            if (stage == null)
                return NotFound(new { message = "Stage not found" });

            return _responseHandler.Success(stage);
        }

        [HttpGet("{id}/criteria")]
        public async Task<IActionResult> GetCriteriaByStageId(int id)
        {
            var result = await _stageService.GetCriteriaByStageIdAsync(id);
            if (result == null || result.Count == 0)
                return NotFound(new { message = "No criteria found for this stage." });

            return _responseHandler.Success(result);
        }
        [HttpPost("{id}/criteria")]
        public async Task<IActionResult> AddCriterion(int id, [FromBody] CreateEvaluationCriteriaDTO dto)
        {
            var result = await _stageService.AddCriterionAsync(id, dto);
            if (result == null)
                return NotFound(new { message = "Stage not found" });

            return _responseHandler.Created(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStage(int id, [FromBody] UpdateStageDTO dto)
        {
            var success = await _stageService.UpdateStageAsync(id, dto);
            if (!success)
                return NotFound(new { message = "Stage not found." });

            return _responseHandler.Success(message: "Stage updated successfully.");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteStage(int id)
        {
            var result = await _stageService.SoftDeleteStageAsync(id);
            if (!result)
                return NotFound(new { message = "Stage not found." });

            return _responseHandler.Success(message: "Stage deactivated (soft deleted) successfully.");
        }

        [HttpPut("{id}/restore")]
        public async Task<IActionResult> RestoreStage(int id)
        {
            var result = await _stageService.RestoreStageAsync(id);

            return result switch
            {
                "Stage not found" => NotFound(new { message = result }),
                "Stage is already active" => BadRequest(new { message = result }),
                _ => _responseHandler.Success(message: result)
            };
        }


    }
}
