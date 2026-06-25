using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domain.Interfaces;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Models;

namespace Backend.Application.UseCases.Comments;

public class DeleteCommentHandler : ICommandHandler<DeleteCommentCommand, Unit>
{
    private readonly ICommentInterface _commentRepository; 

    public DeleteCommentHandler(ICommentInterface commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<Unit> HandleAsync(DeleteCommentCommand command, CancellationToken cancellationToken = default)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(command.CommentId);

        if (comment == null)
            throw new KeyNotFoundException($"Kommentar mit ID {command.CommentId} nicht gefunden.");

        if (comment.AuthorId != command.CurrentUserId /* && !IsCurrentUserAdmin() */) 
            throw new UnauthorizedAccessException("Sie haben keine Berechtigung, diesen Kommentar zu löschen.");

        comment.IsDeleted = true;
        
        await _commentRepository.UpdateCommentAsync(comment);

        return Unit.Value;
    }
}