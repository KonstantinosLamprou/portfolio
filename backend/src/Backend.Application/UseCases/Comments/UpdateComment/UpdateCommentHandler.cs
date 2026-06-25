using Backend.Domain.Interfaces;
using Backend.Domain.Contracts;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Models;


namespace Backend.Application.UseCases.Comments;

public class UpdateCommentHandler : ICommandHandler<UpdateCommentCommand, Unit>
{
    private readonly ICommentInterface _commentRepository; 

    public UpdateCommentHandler(ICommentInterface commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<Unit> HandleAsync(UpdateCommentCommand command, CancellationToken cancellationToken = default)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(command.CommentId); 

        if (comment == null)
            throw new KeyNotFoundException($"Kommentar mit ID {command.CommentId} nicht gefunden.");

        // Berechtigungsprüfung: Nur der Autor oder ein Admin darf bearbeiten
        if (comment.AuthorId != command.CurrentUserId /* && !IsCurrentUserAdmin() */) 
            throw new UnauthorizedAccessException("Sie haben keine Berechtigung, diesen Kommentar zu bearbeiten.");

        comment.Text = command.Request.Text;

        await _commentRepository.UpdateCommentAsync(comment);

        return Unit.Value;
    }
}