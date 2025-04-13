using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.Application.Services;
using SkillAssessmentPlatform.API.Common;
using Microsoft.AspNetCore.Authorization;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Entities;

namespace SkillAssessmentPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelsController : ControllerBase
    {
        private readonly LevelService _levelService;
        private readonly IResponseHandler _responseHandler;

        public LevelsController(LevelService levelService, IResponseHandler responseHandler)
        {
            _levelService = levelService;
          
            _responseHandler = responseHandler;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var level = await _levelService.GetLevelsByLevelIdAsync(id);
            return _responseHandler.Success(level);
        }

        [HttpPut("{id}")]
      //  [Authorize(Roles = "Admin,Senior")] 
        public async Task<IActionResult> UpdateLevel(int id, [FromBody] UpdateLevelDto dto)
        {
            var updated = await _levelService.UpdateLevelAsync(id,dto);
            return _responseHandler.Success(updated, "Track updated successfully");
          
        }
        [HttpGet("{id}/stages")]
        public async Task<IActionResult> GetStagesByLevelId(int id)
        {
            var stages = await _levelService.GetStagesByLevelIdAsync(id);
            if (stages == null)
                return NotFound(new { message = "Level not found or has no stages." });

            return _responseHandler.Success(stages);
        }

        /*  [HttpGet("{id}/stages")]
          public async Task<IActionResult> GetStagesByLevelId(int id)
          {

              var stages = await _levelService.GetStagesByLevelIdAsync(id);
              if (stages == null)
                  return NotFound(new { message = "Level not found or has no stages." });

              return _responseHandler.Success(stages);
          }
        */


    }
}
