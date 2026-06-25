using Microsoft.AspNetCore.Mvc;
using Backend.Application.UseCases.Guestbook;
using Backend.Domain.Contracts;
using Backend.Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Models;

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/guestbook")]
public class GuestbookController : ControllerBase
{
    private readonly IQueryHandler<GetGuestbookEntriesQuery, IEnumerable<UserGuestbookEntryResponse>> _getGuestbookEntriesHandler;
    private readonly ICommandHandler<CreateGuestbookEntryCommand, UserGuestbookEntryResponse> _createGuestbookEntryHandler;
    private readonly ICommandHandler<UpdateGuestbookEntryCommand, UserGuestbookEntryResponse> _updateGuestbookEntryHandler;
    private readonly ICommandHandler<DeleteGuestbookEntryCommand, Unit> _deleteGuestbookEntryHandler;
    private readonly IQueryHandler<GetGuestbookEntryQuery, UserGuestbookEntryResponse> _getGuestbookEntryHandler;

    public GuestbookController(
        IQueryHandler<GetGuestbookEntriesQuery, IEnumerable<UserGuestbookEntryResponse>> getGuestbookEntriesHandler, 
        ICommandHandler<CreateGuestbookEntryCommand, UserGuestbookEntryResponse> createGuestbookEntryHandler,
        ICommandHandler<UpdateGuestbookEntryCommand, UserGuestbookEntryResponse> updateGuestbookEntryHandler, 
        ICommandHandler<DeleteGuestbookEntryCommand, Unit> deleteGuestbookEntryHandler,
        IQueryHandler<GetGuestbookEntryQuery, UserGuestbookEntryResponse> getGuestbookEntryHandler)
    {
        _getGuestbookEntriesHandler = getGuestbookEntriesHandler;
        _createGuestbookEntryHandler = createGuestbookEntryHandler;
        _updateGuestbookEntryHandler = updateGuestbookEntryHandler;
        _deleteGuestbookEntryHandler = deleteGuestbookEntryHandler;
        _getGuestbookEntryHandler = getGuestbookEntryHandler;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserGuestbookEntryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGuestbookEntries(CancellationToken cancellationToken = default)
    {
        var entries = await _getGuestbookEntriesHandler.HandleAsync(new GetGuestbookEntriesQuery(), cancellationToken);

        return Ok(entries ?? Enumerable.Empty<UserGuestbookEntryResponse>());
    }

    [HttpGet("{entryId}")]
    [ProducesResponseType(typeof(UserGuestbookEntryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGuestbookEntry(
        [FromRoute] Guid entryId,
        CancellationToken cancellationToken = default)
    {
        var entry = await _getGuestbookEntryHandler.HandleAsync(new GetGuestbookEntryQuery(entryId), cancellationToken);

        if (entry == null)
            return NotFound();

        return Ok(entry);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(UserGuestbookEntryResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateGuestbookEntry(
        [FromBody] CreateGuestbookEntryRequest request, 
        CancellationToken cancellationToken = default)
    {
        var userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User);

        if (userId == null || userId == Guid.Empty)
            return Unauthorized();

        var entry = await _createGuestbookEntryHandler.HandleAsync(new CreateGuestbookEntryCommand(request, userId.Value), cancellationToken);

        return CreatedAtAction(nameof(GetGuestbookEntry), new { entryId = entry.Id }, entry);
    }

    [HttpPatch("{entryId}")]
    [Authorize]
    [ProducesResponseType(typeof(UserGuestbookEntryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateGuestbookEntry(
        [FromRoute] Guid entryId, 
        [FromBody] UpdateGuestbookEntryRequest request, 
        CancellationToken cancellationToken = default)
    {
        var userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User);

        if (userId == null || userId == Guid.Empty)
            return Unauthorized();

        var entry = await _updateGuestbookEntryHandler.HandleAsync(new UpdateGuestbookEntryCommand(entryId, request.Message, userId.Value), cancellationToken);

        return Ok(entry);
    }

    [HttpDelete("{entryId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize]
    public async Task<IActionResult> DeleteGuestbookEntry(
        [FromRoute] Guid entryId, 
        CancellationToken cancellationToken = default)
    {
        var userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User);

        if (userId == null || userId == Guid.Empty)
            return Unauthorized();

        await _deleteGuestbookEntryHandler.HandleAsync(new DeleteGuestbookEntryCommand(entryId, userId.Value), cancellationToken);
        return NoContent();
    }
}