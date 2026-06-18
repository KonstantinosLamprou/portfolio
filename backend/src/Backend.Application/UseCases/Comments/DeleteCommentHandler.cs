using Backend.Domain.Interfaces;
using Backend.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Application.UseCases.Comments;

public class DeleteCommentHandler
{
    private readonly ICommentInterface _commentRepository; 

    public DeleteCommentHandler(ICommentInterface commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<bool> Handle(Guid commentId, Guid currentUserId)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(commentId);

        if (comment == null)
            throw new KeyNotFoundException($"Kommentar mit ID {commentId} nicht gefunden.");

        if (comment.AuthorId != currentUserId /* && !IsCurrentUserAdmin() */) 
            throw new UnauthorizedAccessException("Sie haben keine Berechtigung, diesen Kommentar zu löschen.");

        comment.IsDeleted = true;
        
        await _commentRepository.UpdateCommentAsync(comment);

        return true;
    }
}