using Backend.Domain.Contracts;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;

namespace Backend.Application.UseCases.Interactions;

public class UpdateCommentVoteHandler
{
    private readonly ICommentVoteInterface _repository;

    public UpdateCommentVoteHandler(ICommentVoteInterface repository)
    {
        _repository = repository;
    }

    public async Task Handle(Guid commentId, UpdateVoteDto request, Guid userId)
    {
        if (request.CommentId != commentId)
            throw new ArgumentException("Die Kommentar-ID in der URL stimmt nicht mit der ID im Request-Body überein.");
        
        var existingVote = await _repository.GetCommentVotesAsync(request.CommentId);

        if (!existingVote.Any(c => c.UserId == userId))
        {
            throw new InvalidOperationException("Keine bestehende Bewertung gefunden, die aktualisiert werden könnte.");
        }

        await _repository.UpdateCommentVoteAsync(request.IsUpvote, request.CommentId, userId);
    }
}