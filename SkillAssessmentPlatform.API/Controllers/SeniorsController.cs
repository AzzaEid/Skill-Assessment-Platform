using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Services;

namespace SkillAssessmentPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SeniorsController : ControllerBase
    {
        private readonly SeniorService _seniorService;
        private readonly IResponseHandler _responseHandler;

        public SeniorsController(SeniorService seniorService, IResponseHandler responseHandler)
        {
            _seniorService = seniorService;
            _responseHandler = responseHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _seniorService.GetAllSeniorsAsync();
            if (result == null)
            {
                return _responseHandler.NotFound();
            }
            return _responseHandler.Success(result);
        }

        [HttpGet("track/{trackId}")]
        public async Task<IActionResult> GetSeniorByTrack(int trackId)
        {
            var result = await _seniorService.GetSeniorByTrackIdAsync(trackId);
            return _responseHandler.Success(result);
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignSenior([FromBody] AssignSeniorDTO dto)
        {
            var result = await _seniorService.AssignSeniorAsync(dto.ExaminerId, dto.TrackId);
            return result ? _responseHandler.Success() : _responseHandler.BadRequest("Failed to assign senior");
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateSenior([FromBody] AssignSeniorDTO dto)
        {
            var result = await _seniorService.UpdateSeniorAsync(dto.ExaminerId, dto.TrackId);
            return result ? _responseHandler.Success() : _responseHandler.BadRequest("Failed to update senior");
        }

        [HttpDelete("{trackId}")]
        public async Task<IActionResult> RemoveSenior(int trackId)
        {
            var result = await _seniorService.RemoveSeniorAsync(trackId);
            return result ? _responseHandler.Success() : _responseHandler.BadRequest("Failed to remove senior");
        }
    }

}
