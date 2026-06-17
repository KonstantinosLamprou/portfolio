using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Backend.Application.UseCases.Interactions;
using Backend.Api.Helpers;
using Backend.Domain.Contracts;

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/commentvote")]
public class CommentVoteController : ControllerBase
{
    private readonly CreateCommentVoteHandler _createCommentVoteHandler;
    private readonly UpdateCommentVoteHandler _updateCommentVoteHandler;
    private readonly DeleteCommentVoteHandler _deleteCommentVoteHandler;
    private readonly GetCommentsVoteHandler _getCommentsVoteHandler;

    public CommentVoteController(
        CreateCommentVoteHandler createCommentVoteHandler,
        UpdateCommentVoteHandler updateCommentVoteHandler,
        DeleteCommentVoteHandler deleteCommentVoteHandler,
        GetCommentsVoteHandler getCommentsVoteHandler)
    {
        _createCommentVoteHandler = createCommentVoteHandler;
        _updateCommentVoteHandler = updateCommentVoteHandler;
        _deleteCommentVoteHandler = deleteCommentVoteHandler;
        _getCommentsVoteHandler = getCommentsVoteHandler;
    }

    [HttpGet("{commentId}")]
    public async Task<IActionResult> GetCommentsVote(Guid commentId)
    {
            var result = await _getCommentsVoteHandler.Handle(commentId);

            return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateVoteDto), StatusCodes.Status201Created)]
    [Authorize]
    public async Task<IActionResult> CreateOrUpdateCommentVote([FromBody] CreateVoteDto createVoteDto)
    {
        try
        {
            var userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User);

            if (userId == Guid.Empty || userId == null)
                return Unauthorized();

            await _createCommentVoteHandler.Handle(createVoteDto, userId.Value);
            return CreatedAtAction(nameof(GetCommentsVote), new { commentId = createVoteDto.CommentId }, createVoteDto); 
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{commentId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize]
    public async Task<IActionResult> UpdateCommentVote(Guid commentId, [FromBody] UpdateVoteDto updateVoteDto)
    {
        try
        {
            var userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User);

            if (userId == Guid.Empty || userId == null)
                return Unauthorized();

            await _updateCommentVoteHandler.Handle(commentId, updateVoteDto, userId.Value);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{commentId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize]
    public async Task<IActionResult> DeleteCommentVote(Guid commentId)
    {
        try
        {
            var userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User);

            if (userId == Guid.Empty || userId == null)
                return Unauthorized();

            var result = await _deleteCommentVoteHandler.Handle(commentId, userId.Value);

            if (!result)
                return NotFound(new { message = "Keine Bewertung gefunden, die gelöscht werden könnte." });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}