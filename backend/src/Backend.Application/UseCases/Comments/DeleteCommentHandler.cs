using Backend.Domain.Interfaces;
using Backend.Domain.Contracts;


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

        // Berechtigungsprüfung: Nur der Autor oder ein Admin darf löschen
        if (comment.AuthorId != currentUserId /* && !IsCurrentUserAdmin() */) 
            throw new UnauthorizedAccessException("Sie haben keine Berechtigung, diesen Kommentar zu löschen.");

        return await _commentRepository.DeleteCommentAsync(comment.Id);
    }
}