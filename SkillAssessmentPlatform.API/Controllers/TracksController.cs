using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Services;

[ApiController]
[Route("api/[controller]")]
public class TracksController : ControllerBase
{
    private readonly TrackService _trackService;
    private readonly IResponseHandler _responseHandler;

    public TracksController(TrackService trackService, IResponseHandler responseHandler)
    {
        _trackService = trackService;
        _responseHandler = responseHandler;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var track = await _trackService.GetTrackByIdAsync(id);
        return _responseHandler.Success(track);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTracksAsync()
    {
        var result = await _trackService.GetAllTracksAsync();
        return _responseHandler.Success(result);
    }
    [HttpGet("/not-active")]
    public async Task<IActionResult> GetNotActiveTracks()
    {
        var result = await _trackService.GetNotActiveTracksAsync();
        return _responseHandler.Success(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTrackDTO dto) =>
     _responseHandler.Created(await _trackService.CreateTrackAsync(dto));

    [HttpPost("structure")]
    public async Task<IActionResult> CreateTrackStructure([FromBody] TrackStructureDTO structureDTO)
    {
        try
        {
            var result = await _trackService.CreateTrackStructureAsync(structureDTO);
            return _responseHandler.Success<string>("Track structure created successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return _responseHandler.NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return _responseHandler.BadRequest($" errors in creating {ex.Message}");
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] TrackDto dto) =>
        _responseHandler.Success(await _trackService.UpdateTrackAsync(dto), "Track updated successfully");


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeActivateTrackAsync(int id)
    {
        await _trackService.DeActivateTrackAsync(id);
        return _responseHandler.Deleted();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> ActivateTrackAsync(int id)
    {
        await _trackService.ActivateTrackAsync(id);
        return _responseHandler.Deleted();
    }

    //[HttpPost("{trackId}/levels")]
    //public async Task<IActionResult> CreateLevel(int trackId, [FromBody] CreateLevelDTO dto)
    //{
    //    var created = await _trackService.CreateLevelAsync(trackId, dto);
    //    if (created == null)
    //        return NotFound(new { message = "Track not found" });

    //    return _responseHandler.Success(created, "Level created successfully");
    //}
}
