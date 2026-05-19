using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Backend.Application.UseCases.Interactions;

namespace Backend.Presentation.Controllers;

[ApiController]
[Route("api/content")]
public class InteractionController : ControllerBase
{
    private readonly AddLikeHandler _addLikeHandler;

    public InteractionController(AddLikeHandler addLikeHandler)
    {
        _addLikeHandler = addLikeHandler;
    }

    [HttpPost("{contentId}/like")]
    [Authorize] // Wichtig: Nur eingeloggte User dürfen liken!
    public async Task<IActionResult> LikeContent(int contentId)
    {
        try
        {
            // Sichere User-ID aus dem Token auslesen
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = Guid.Parse(userIdString!);

            // Handler feuern
            var newLikeCount = await _addLikeHandler.Handle(contentId, userId);

            // Wir geben die NEUE Anzahl an das Frontend zurück (z.B. "2"), 
            // damit Vue das sofort anzeigen kann.
            return Ok(new { currentLikeCount = newLikeCount });
        }
        catch (InvalidOperationException ex)
        {
            // Wenn der User über 3 Likes geht, werfen wir einen 400 Bad Request
            return BadRequest(new { message = ex.Message });
        }
    }
}