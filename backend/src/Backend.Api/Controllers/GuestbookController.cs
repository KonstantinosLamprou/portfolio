using Microsoft.AspNetCore.Mvc;
using Backend.Application.UseCases.Guestbook;
using Backend.Domain.Contracts;
using Backend.Api.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/guestbook")]
public class GuestbookController : ControllerBase
{
    private readonly GetGuestbookEntriesHandler _getGuestbookEntriesHandler;
    private readonly CreateGuestbookEntryHandler _createGuestbookEntryHandler;
    private readonly UpdateGuestbookEntryHandler _updateGuestbookEntryHandler;
    private readonly DeleteGuestbookEntryHandler _deleteGuestbookEntryHandler;
    private readonly GetGuestbookEntryHandler _getGuestbookEntryHandler;

    public GuestbookController(
        GetGuestbookEntriesHandler getGuestbookEntriesHandler, 
        CreateGuestbookEntryHandler createGuestbookEntryHandler,
        UpdateGuestbookEntryHandler updateGuestbookEntryHandler, 
        DeleteGuestbookEntryHandler deleteGuestbookEntryHandler,
        GetGuestbookEntryHandler getGuestbookEntryHandler)
    {
        _getGuestbookEntriesHandler = getGuestbookEntriesHandler;
        _createGuestbookEntryHandler = createGuestbookEntryHandler;
        _updateGuestbookEntryHandler = updateGuestbookEntryHandler;
        _deleteGuestbookEntryHandler = deleteGuestbookEntryHandler;
        _getGuestbookEntryHandler = getGuestbookEntryHandler;
    }

    [HttpGet()]
    [ProducesResponseType(typeof(IEnumerable<UserGuestbookEntryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGuestbookEntries()
    {
        var entries = await _getGuestbookEntriesHandler.Handle();
        return Ok(entries);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserGuestbookEntryResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGuestbookEntry(Guid id)
    {
        var entry = await _getGuestbookEntryHandler.Handle(id);

        if (entry == null)
            return NotFound();

        return Ok(entry);
    }

    [HttpPost("")]
    [Authorize]
    [ProducesResponseType(typeof(UserGuestbookEntryResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateGuestbookEntry([FromBody] CreateGuestbookEntryRequest request)
    {
        var userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User);

        if (userId == null)
            return Unauthorized();

        var entry = await _createGuestbookEntryHandler.Handle(request, userId.Value);

        return CreatedAtAction(nameof(GetGuestbookEntry), new { id = entry.Id }, entry);
    }

    [HttpPatch("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(UserGuestbookEntryResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateGuestbookEntry(Guid id, [FromBody] UpdateGuestbookEntryRequest request)
    {
        var userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User);

        if (userId == null)
            return Unauthorized();

        var entry = await _updateGuestbookEntryHandler.Handle(id, request, userId.Value);

        return Ok(entry);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize]
    public async Task<IActionResult> DeleteGuestbookEntry(Guid id)
    {
        var userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User);

        if (userId == null)
            return Unauthorized();

        await _deleteGuestbookEntryHandler.Handle(id, userId.Value);
        return NoContent();
    }

}