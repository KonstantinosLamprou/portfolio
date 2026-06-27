using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.Application.UseCases.Comments;
using Backend.Domain.Contracts; 
using Backend.Api.Helpers;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Models;

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/comments")]
public class CommentsController : ControllerBase
{
    private readonly ICommandHandler<CreateCommentCommand, CommentResponseDto> _createCommentHandler;
    private readonly IQueryHandler<GetCommentsQuery, List<CommentResponseDto>> _getCommentsHandler;
    private readonly ICommandHandler<UpdateCommentCommand, Unit> _updateCommentHandler;
    private readonly ICommandHandler<DeleteCommentCommand, Unit> _deleteCommentHandler;
    private readonly IQueryHandler<GetCommentByIdQuery, CommentResponseDto?> _getCommentByIdHandler;

    public CommentsController(
        ICommandHandler<CreateCommentCommand, CommentResponseDto> createCommentHandler, 
        IQueryHandler<GetCommentsQuery, List<CommentResponseDto>> getCommentsHandler, 
        ICommandHandler<UpdateCommentCommand, Unit> updateCommentHandler, 
        ICommandHandler<DeleteCommentCommand, Unit> deleteCommentHandler,
        IQueryHandler<GetCommentByIdQuery, CommentResponseDto?> getCommentByIdHandler)
    {
        _createCommentHandler = createCommentHandler;
        _getCommentsHandler = getCommentsHandler;
        _updateCommentHandler = updateCommentHandler;
        _deleteCommentHandler = deleteCommentHandler;
        _getCommentByIdHandler = getCommentByIdHandler;
    }

    [HttpGet("{commentId}")]
    [ProducesResponseType(typeof(CommentResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetComment(
        [FromRoute] Guid commentId, 
        CancellationToken cancellationToken)
    {
        Guid userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User) ?? Guid.Empty;

        var comment = await _getCommentByIdHandler.HandleAsync(
            new GetCommentByIdQuery(commentId, userId), 
            cancellationToken
            );

        if (comment == null)
            return NotFound();

        return Ok(comment);
    }

    [HttpPost]
    [Authorize] 
    public async Task<IActionResult> CreateComment(
        [FromBody] CreateCommentRequest request, 
        CancellationToken cancellationToken)
    {
        // ID aus den Token-Claims des angemeldeten Users holen
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (!Guid.TryParse(userIdString, out Guid authorId))
            return Unauthorized();

        var responseDto = await _createCommentHandler.HandleAsync(new CreateCommentCommand(request, authorId), cancellationToken);

        return CreatedAtAction(nameof(GetComment), new { commentId = responseDto.Id }, responseDto); 
    
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CommentResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetComments(
        [FromQuery] int contentId, 
        [FromQuery] string contentType, 
        CancellationToken cancellationToken)
    {

        Guid userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User) ?? Guid.Empty;

        //Testen ob das hier so funktioniert ohne type 
        var comments = await _getCommentsHandler.HandleAsync(
            new GetCommentsQuery(contentId, contentType, userId), 
            cancellationToken);

        return Ok(comments);
    }

    [HttpPatch("{commentId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize]
    public async Task<IActionResult> UpdateComment(
        Guid commentId, 
        [FromBody] UpdateCommentRequest request, 
        [FromQuery] int contentId, 
        CancellationToken cancellationToken)
    {
        var userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User);

        if (userId == null)
            return Unauthorized();

        await _updateCommentHandler.HandleAsync(
            new UpdateCommentCommand(commentId, contentId, request, userId.Value), 
            cancellationToken);

        return NoContent();
    }
    
    [HttpDelete("{commentId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize]
    public async Task<IActionResult> DeleteComment(
        Guid commentId, 
        [FromQuery] int contentId, 
        CancellationToken cancellationToken) 
    {
        var userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User);
        if (userId == null) 
            return Unauthorized();

        await _deleteCommentHandler.HandleAsync(
            new DeleteCommentCommand(commentId, contentId, userId.Value), 
            cancellationToken);

        return NoContent();
    }


}
