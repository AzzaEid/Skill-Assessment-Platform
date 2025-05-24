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
    [HttpGet("{id}/structure")]
    public async Task<IActionResult> GetStructureById(int id)
    {
        var track = await _trackService.GetTrackStructure(id);
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
    // fromform
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateTrackDTO dto) =>
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
            // return _responseHandler.BadRequest($" errors in creating {ex.Message}");
            return BadRequest($" errors in creating {ex.Message} | Inner: {ex.InnerException?.Message}");

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
    [HttpGet("active")]
    public async Task<IActionResult> GetOnlyActiveTracks()
    {
        var result = await _trackService.GetOnlyActiveTracksAsync();
        return _responseHandler.Success(result);
    }
    [HttpGet("summary")]
    public async Task<IActionResult> GetTracksSummary()
    {
        var result = await _trackService.GetAllTracksSummaryAsync();
        return _responseHandler.Success(result);
    }

    [HttpGet("not-active")]
    public async Task<IActionResult> GetOnlyDeactivatedTracks()
    {
        var result = await _trackService.GetOnlyDeactivatedTracksAsync();
        return _responseHandler.Success(result);
    }


    [HttpPut("{id}/restore")]
    public async Task<IActionResult> RestoreTrack(int id)
    {
        var result = await _trackService.RestoreTrackAsync(id);

        return result switch
        {
            "Track not found" => NotFound(new { message = result }),
            "Track is already active" => BadRequest(new { message = result }),
            _ => _responseHandler.Success(message: result)
        };
    }

    [HttpGet("{trackId}/examiners")]
    public async Task<IActionResult> GetWorkingExaminers(int trackId)
    {
        var result = await _trackService.GetWorkingExaminersByTrackIdAsync(trackId);
        return _responseHandler.Success(result, "Working examiners fetched successfully");
    }

    [HttpGet("{trackId}/examiners-summary")]
    public async Task<IActionResult> GetWorkingExaminersList(int trackId)
    {
        var result = await _trackService.GetTrackWorkingExaminersAsync(trackId);
        return _responseHandler.Success(result, "Working examiners fetched successfully");
    }

    [HttpGet("active-list")]
    public async Task<IActionResult> GetActiveTrackList()
    {
        var result = await _trackService.GetActiveTrackListAsync();
        return _responseHandler.Success(result, "Active tracks fetched successfully");
    }


    [HttpPost("{trackId}/levels")]
    public async Task<IActionResult> AddLevelToTrack(int trackId, [FromBody] CreateLevelDTO dto)
    {
        var success = await _trackService.AddLevelToTrackAsync(trackId, dto);
        if (!success)
            return NotFound($"Track with ID {trackId} not found.");

        return Ok("Level added successfully to track.");
    }

}
