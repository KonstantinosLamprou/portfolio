using Backend.Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend.Application.UseCases.Interactions;

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/statistics")]
public class StatisticsController : ControllerBase
{
    private readonly GetStatisticsHandler _getStatisticsHandler;
    private readonly UpdateStatisticsHandler _updateStatisticsHandler;
    public StatisticsController(GetStatisticsHandler getStatisticsHandler, UpdateStatisticsHandler updateStatisticsHandler)
    {
        _getStatisticsHandler = getStatisticsHandler;
        _updateStatisticsHandler = updateStatisticsHandler;
    }
        
    [HttpGet]
    [ProducesResponseType(typeof(StatisticsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<StatisticsResponse>?> GetStatistics()
    {
        var stats = await _getStatisticsHandler.Handle();
        
        if (stats == null)
            return NotFound();

        return Ok(stats);
    }


    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateStatistics([FromBody] UpdateStatisticsRequest request)
    {
        await _updateStatisticsHandler.Handle(request.ViewToAdd, request.LikeToAdd);
        return NoContent();
    }
}