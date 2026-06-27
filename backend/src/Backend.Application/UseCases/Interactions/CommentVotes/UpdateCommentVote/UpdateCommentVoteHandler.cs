using Backend.Domain.Contracts;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Models;

namespace Backend.Application.UseCases.Interactions;

public class UpdateCommentVoteHandler : ICommandHandler<UpdateCommentVoteCommand, Unit>
{
    private readonly ICommentVoteInterface _repository;

    public UpdateCommentVoteHandler(ICommentVoteInterface repository)
    {
        _repository = repository;
    }

    public async Task<Unit> HandleAsync(UpdateCommentVoteCommand command, CancellationToken cancellationToken)
    {
        var request = new UpdateVoteDto(command.IsUpvote, command.CommentId, command.ContentId);
        var userId = command.CurrentUserId;

        if (request.CommentId != command.CommentId)
            throw new ArgumentException("Die Kommentar-ID in der URL stimmt nicht mit der ID im Request-Body überein.");
        
        var existingVote = await _repository.GetCommentVotesAsync(request.CommentId, cancellationToken);

        if (!existingVote.Any(c => c.UserId == userId))
        {
            throw new InvalidOperationException("Keine bestehende Bewertung gefunden, die aktualisiert werden könnte.");
        }

        await _repository.UpdateCommentVoteAsync(request.IsUpvote, request.CommentId, userId, cancellationToken);

        return Unit.Value;
    }
}