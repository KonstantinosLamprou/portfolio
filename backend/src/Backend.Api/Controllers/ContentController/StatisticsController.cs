using Backend.Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Models;

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/statistics")]
public class StatisticsController : ControllerBase
{
    private readonly IQueryHandler<GetStatisticsQuery, StatisticsResponse> _getStatisticsHandler;
    private readonly ICommandHandler<UpdateStatisticsCommand, Unit> _updateStatisticsHandler;
    public StatisticsController(IQueryHandler<GetStatisticsQuery, StatisticsResponse> getStatisticsHandler, ICommandHandler<UpdateStatisticsCommand, Unit> updateStatisticsHandler)
    {
        _getStatisticsHandler = getStatisticsHandler;
        _updateStatisticsHandler = updateStatisticsHandler;
    }
        
    [HttpGet]
    [ProducesResponseType(typeof(StatisticsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StatisticsResponse>?> GetStatistics(CancellationToken cancellationToken)
    {
        var stats = await _getStatisticsHandler.HandleAsync(new GetStatisticsQuery(), cancellationToken);
        
        if (stats == null)
            return NotFound();

        return Ok(stats);
    }


    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateStatistics([FromBody] UpdateStatisticsRequest request, CancellationToken cancellationToken)
    {
        await _updateStatisticsHandler.HandleAsync(new UpdateStatisticsCommand(request.ViewToAdd, request.LikeToAdd), cancellationToken);

        return NoContent();
    }
}