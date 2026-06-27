using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Backend.Application.UseCases.Interactions;
using Backend.Api.Helpers;
using Backend.Domain.Contracts;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Models;


namespace Backend.Api.Controllers;

[ApiController]
[Route("api/commentvote")]
public class CommentVoteController : ControllerBase
{
    private readonly ICommandHandler<CreateCommentVoteCommand, Unit> _createCommentVoteHandler;
    private readonly ICommandHandler<UpdateCommentVoteCommand, Unit> _updateCommentVoteHandler;
    private readonly ICommandHandler<DeleteCommentVoteCommand, Unit> _deleteCommentVoteHandler;
    private readonly IQueryHandler<GetCommentsVoteQuery, Dictionary<string, int>> _getCommentsVoteHandler;

    public CommentVoteController(
        ICommandHandler<CreateCommentVoteCommand, Unit> createCommentVoteHandler,
        ICommandHandler<UpdateCommentVoteCommand, Unit> updateCommentVoteHandler,
        ICommandHandler<DeleteCommentVoteCommand, Unit> deleteCommentVoteHandler,
        IQueryHandler<GetCommentsVoteQuery, Dictionary<string, int>> getCommentsVoteHandler)
    {
        _createCommentVoteHandler = createCommentVoteHandler;
        _updateCommentVoteHandler = updateCommentVoteHandler;
        _deleteCommentVoteHandler = deleteCommentVoteHandler;
        _getCommentsVoteHandler = getCommentsVoteHandler;
    }

    [HttpGet("{commentId}")]
    public async Task<IActionResult> GetCommentsVote([FromRoute] Guid commentId, CancellationToken cancellationToken)
    {
        var userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User) ?? Guid.Empty;

        var result = await _getCommentsVoteHandler.HandleAsync(new GetCommentsVoteQuery(commentId, userId), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateVoteDto), StatusCodes.Status201Created)]
    [Authorize]
    public async Task<IActionResult> CreateOrUpdateCommentVote([FromBody] CreateVoteDto createVoteDto, CancellationToken cancellationToken)
    {
        var userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User);

        if (userId == Guid.Empty || userId == null)
            return Unauthorized();

        await _createCommentVoteHandler.HandleAsync(new CreateCommentVoteCommand(createVoteDto, userId.Value), cancellationToken);

        return CreatedAtAction(nameof(GetCommentsVote), new { commentId = createVoteDto.CommentId }, createVoteDto); 

    }

    [HttpPatch("{commentId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize]
    public async Task<IActionResult> UpdateCommentVote([FromRoute] Guid commentId, [FromBody] UpdateVoteDto updateVoteDto, CancellationToken cancellationToken)
    {
        var userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User);

        if (userId == Guid.Empty || userId == null)
            return Unauthorized();

        await _updateCommentVoteHandler.HandleAsync(new UpdateCommentVoteCommand(commentId, updateVoteDto.IsUpvote, updateVoteDto.ContentId, userId.Value), cancellationToken);
        
        return NoContent();

    }

    [HttpDelete("{commentId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize]
    public async Task<IActionResult> DeleteCommentVote([FromRoute] Guid commentId, [FromBody] int contentId, CancellationToken cancellationToken)
    {

        var userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User);

        if (userId == Guid.Empty || userId == null)
            return Unauthorized();

        await _deleteCommentVoteHandler.HandleAsync(new DeleteCommentVoteCommand(commentId, contentId, userId.Value), cancellationToken);

        return NoContent();

    }
}