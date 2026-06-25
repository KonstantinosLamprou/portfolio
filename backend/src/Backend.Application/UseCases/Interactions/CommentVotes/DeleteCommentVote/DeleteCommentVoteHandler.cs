using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Models;

namespace Backend.Application.UseCases.Interactions;

public class DeleteCommentVoteHandler : ICommandHandler<DeleteCommentVoteCommand, Unit>
{
    private readonly ICommentVoteInterface _repository;

    public DeleteCommentVoteHandler(ICommentVoteInterface repository)
    {
        _repository = repository;
    }

    public async Task<Unit> HandleAsync(DeleteCommentVoteCommand command, CancellationToken cancellationToken = default)
    {

        var existingVote = await _repository.GetCommentVotesAsync(command.CommentId, cancellationToken);
        
        if (!existingVote.Any(c => c.UserId == command.CurrentUserId))
        {
            return Unit.Value; // keine vote um zu löschen, einfach zurückgeben
        }

        await _repository.DeleteCommentVoteAsync(command.CommentId, command.CurrentUserId, cancellationToken);

        return Unit.Value;
    }
}