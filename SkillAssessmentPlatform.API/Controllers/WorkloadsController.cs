using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Services;
using SkillAssessmentPlatform.Core.Exceptions;

namespace SkillAssessmentPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkloadsController : ControllerBase
    {

        private readonly ExaminerLoadsService _workloadService;
        private readonly IResponseHandler _responseHandler;

        public WorkloadsController(
            ExaminerLoadsService workloadService,
            IResponseHandler responseHandler)
        {
            _workloadService = workloadService;
            _responseHandler = responseHandler;
        }

        [HttpGet("examiner/{examinerId}")]
        public async Task<IActionResult> GetExaminerWorkloads(string examinerId)
        {
            var workloads = await _workloadService.GetByExaminerIdAsync(examinerId);
            return _responseHandler.Success(workloads);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkloadById(int id)
        {
            var workload = await _workloadService.GetByIdAsync(id);
            return _responseHandler.Success(workload);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SeniorExaminer")]
        public async Task<IActionResult> CreateWorkload([FromBody] CreateExaminerLoadListDTO createDto)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid data", GetModelStateErrors());
            }

            var createdWorkload = await _workloadService.CreateExaminerLoadAsync(createDto);
            return _responseHandler.Success(createdWorkload, "Workload created successfully");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,SeniorExaminer")]
        public async Task<IActionResult> UpdateWorkload(int id, [FromBody] UpdateWorkLoadDTO updateDto)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid data", GetModelStateErrors());
            }

            var updatedWorkload = await _workloadService.UpdateWorkLoadAsync(id, updateDto);
            return _responseHandler.Success(updatedWorkload, "Workload updated successfully");
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SeniorExaminer")]
        public async Task<IActionResult> DeleteWorkload(int id)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid data", GetModelStateErrors());
            }

            await _workloadService.DeleteLoad(id);
            return _responseHandler.Deleted();
        }

        private List<string> GetModelStateErrors()
        {
            return ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
        }
    }
}
