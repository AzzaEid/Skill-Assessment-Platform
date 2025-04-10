using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.Application.Services;
using SkillAssessmentPlatform.API.Common;




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



    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTrackDTO dto)
    {
        var created = await _trackService.CreateTrackAsync(dto);
        return _responseHandler.Created(created);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] CreateTrackDTO dto)
    {
        var updated = await _trackService.UpdateTrackAsync(dto);
        return _responseHandler.Success(updated, "Track updated successfully");
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeActivateTrackAsync(int id)
    {
        await _trackService.DeActivateTrackAsync(id);
        return _responseHandler.Deleted();
    }
}