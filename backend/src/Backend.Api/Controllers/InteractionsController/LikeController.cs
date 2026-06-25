using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend.Application.UseCases.Interactions;
using Backend.Domain.Contracts;
using Backend.Api.Helpers;
using Backend.Application.Common.Interfaces; 

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/like")]
public class LikeController : ControllerBase
{
    private readonly ICommandHandler<AddLikeCommand, int> _addLikeHandler;

    public LikeController(ICommandHandler<AddLikeCommand, int> addLikeHandler)
    {
        _addLikeHandler = addLikeHandler;
    }

    [HttpPost("{contentId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LikeContent(
        [FromRoute] int contentId,               
        [FromBody] CreateLikeOnContent request,  
        CancellationToken cancellationToken)
    {
        var userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User);
        
        if (userId == null) 
            return Unauthorized();

        var command = new AddLikeCommand(contentId, userId.Value, request.ContentType, request.Slug);
        
        var newLikeCount = await _addLikeHandler.HandleAsync(command, cancellationToken);

        return Ok(new { currentLikeCount = newLikeCount });
    }
}