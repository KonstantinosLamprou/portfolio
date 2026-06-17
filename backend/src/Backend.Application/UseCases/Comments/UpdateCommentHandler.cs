using Backend.Domain.Interfaces;
using Backend.Domain.Contracts;


namespace Backend.Application.UseCases.Comments;

public class UpdateCommentHandler
{
    private readonly ICommentInterface _commentRepository; 

    public UpdateCommentHandler(ICommentInterface commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task Handle(Guid commentId, UpdateCommentRequest request, Guid currentUserId)
    {
        if (request.CommentId != commentId)
            throw new ArgumentException("Die Kommentar-ID in der URL stimmt nicht mit der ID im Request-Body überein.");
        
        var comment = await _commentRepository.GetCommentByIdAsync(request.CommentId); 

        if (comment == null)
            throw new KeyNotFoundException($"Kommentar mit ID {request.CommentId} nicht gefunden.");

        // Berechtigungsprüfung: Nur der Autor oder ein Admin darf bearbeiten
        if (comment.AuthorId != currentUserId /* && !IsCurrentUserAdmin() */) 
            throw new UnauthorizedAccessException("Sie haben keine Berechtigung, diesen Kommentar zu bearbeiten.");

        comment.Text = request.Text;

        await _commentRepository.UpdateCommentAsync(comment);
    }
}