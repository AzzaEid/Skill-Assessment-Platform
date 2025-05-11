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

    //[HttpPost("{trackId}/levels")]
    //public async AppTask<IActionResult> CreateLevel(int trackId, [FromBody] CreateLevelDTO dto)
    //{
    //    var created = await _trackService.CreateLevelAsync(trackId, dto);
    //    if (created == null)
    //        return NotFound(new { message = "Track not found" });

    //    return _responseHandler.Success(created, "Level created successfully");
    //}
}
