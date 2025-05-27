using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Services;
using SkillAssessmentPlatform.API.Common;

namespace SkillAssessmentPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EvaluationCriteriaController : ControllerBase
    {
        private readonly EvaluationCriteriaService _service;
        private readonly IResponseHandler _responseHandler;

        public EvaluationCriteriaController(EvaluationCriteriaService service, IResponseHandler responseHandler)
        {
            _service = service;
            _responseHandler = responseHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEvaluationCriteriaDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return _responseHandler.Success<string>("Evaluation Criteria created");
        }

        [HttpGet("stage/{stageId}")]
        public async Task<IActionResult> GetByStageId(int stageId)
        {
            var list = await _service.GetByStageIdAsync(stageId);
            return _responseHandler.Success(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return _responseHandler.NotFound("Evaluation Criteria not found");

            return _responseHandler.Success(item);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var all = await _service.GetAllAsync();
            return _responseHandler.Success(all);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEvaluationCriteriaDto dto)
        {
            if (id != dto.Id)
                return _responseHandler.BadRequest("ID in URL doesn't match ID in body.");

            var updated = await _service.UpdateAsync(dto);
            if (!updated)
                return _responseHandler.NotFound("Evaluation Criteria not found");

            return _responseHandler.Success<string>("Evaluation Criteria updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var deleted = await _service.SoftDeleteAsync(id);
            if (!deleted)
                return _responseHandler.NotFound("Evaluation Criteria not found or already inactive.");

            return _responseHandler.Deleted();
        }


        [HttpPut("stage/{stageId}/criteria-bulk")]
        public async Task<IActionResult> UpdateStageCriteria([FromBody] UpdateStageCriteriaPayloadDto payload)
        {
            if (payload.StageId <= 0)
                return _responseHandler.BadRequest("Invalid Stage ID.");

            var result = await _service.UpdateStageCriteriaAsync(payload);
            return _responseHandler.Success<string>("Stage evaluation criteria updated successfully.");

            
        }



    }
}
