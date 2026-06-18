using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Backend.Application.UseCases.Interactions;
using Backend.Api.Helpers;  

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/like")]
public class LikeController : ControllerBase
{
    private readonly AddLikeHandler _addLikeHandler;

    public LikeController(AddLikeHandler addLikeHandler)
    {
        _addLikeHandler = addLikeHandler;
    }

    [HttpPost("{contentId}")]
    [Authorize] // Wichtig: Nur eingeloggte User dürfen liken!
    [ProducesResponseType(StatusCodes.Status200OK)
    , ProducesResponseType(StatusCodes.Status400BadRequest)
    , ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LikeContent(int contentId)
    {
        try
        {
            var userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User);

            if (userId == Guid.Empty || userId == null)
                return Unauthorized();

            var newLikeCount = await _addLikeHandler.Handle(contentId, userId.Value);

            return Ok(new { currentLikeCount = newLikeCount });
        }
        catch (InvalidOperationException ex)
        {
            // Wenn der User über 3 Likes geht, werfen wir einen 400 Bad Request
            return BadRequest(new { message = ex.Message });
        }
    }

    
}