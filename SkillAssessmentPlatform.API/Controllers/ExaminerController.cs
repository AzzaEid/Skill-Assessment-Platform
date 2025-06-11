using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.DTOs.Examiner.Input;
using SkillAssessmentPlatform.Application.Services;
using SkillAssessmentPlatform.Core.Entities.Users;

namespace SkillAssessmentPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExaminersController : ControllerBase
    {
        private readonly ExaminerService _examinerService;
        private readonly IResponseHandler _responseHandler;

        public ExaminersController(
            ExaminerService examinerService,
            IResponseHandler responseHandler)
        {
            _examinerService = examinerService;
            _responseHandler = responseHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _examinerService.GetAllExaminersAsync(page, pageSize);
            return _responseHandler.Success(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var examiner = await _examinerService.GetExaminerByIdAsync(id);
            return _responseHandler.Success(examiner);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExaminer(
            string id,
            [FromBody] UpdateExaminerDTO examinerDto)
        {
            var updatedExaminer = await _examinerService.UpdateExaminerAsync(id, examinerDto);
            return _responseHandler.Success(updatedExaminer, "Examiner updated successfully");
        }
        
        [HttpGet("{examinerId}/tracks")]
        public async Task<IActionResult> GetTracks(
            string examinerId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _examinerService.GetExaminerTracksAsync(examinerId);
            return _responseHandler.Success(result);
        }

        [HttpPost("{examinerId}/tracks/{trackId}")]
        public async Task<IActionResult> AddTrack(
            string examinerId,
            int trackId)
        {
            await _examinerService.AddTrackToExaminerAsync(examinerId, trackId);
            return _responseHandler.Success<string>(null,"Track added to examiner");
        }

        [HttpGet("{examinerId}/workload")]
        public async Task<IActionResult> GetWorkload(string examinerId)
        {
            var workload = await _examinerService.GetExaminerWorkloadAsync(examinerId);
            return _responseHandler.Success(workload);
        }
        
        [HttpDelete("{examinerId}/tracks/{trackId}")]
        public async Task<IActionResult> RemoveTrack(
            string examinerId,
            int trackId)
        {
            await _examinerService.RemoveTrackFromExaminerAsync(examinerId, trackId);
            return _responseHandler.Success<string>(null, "Track removed from examiner") ;
        }
    }


}