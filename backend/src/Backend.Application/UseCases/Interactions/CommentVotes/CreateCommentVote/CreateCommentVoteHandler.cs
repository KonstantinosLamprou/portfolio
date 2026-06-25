using Backend.Domain.Contracts;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Models;

namespace Backend.Application.UseCases.Interactions;

public class CreateCommentVoteHandler : ICommandHandler<CreateCommentVoteCommand, Unit>
{
    private readonly ICommentVoteInterface _repository;

    public CreateCommentVoteHandler(ICommentVoteInterface repository)
    {
        _repository = repository;
    }

    public async Task<Unit> HandleAsync(CreateCommentVoteCommand command, CancellationToken cancellationToken = default)
    {
        var commentVote = new CommentVote
        {
            IsUpvote = command.CreateVoteDto.IsUpvote,
            CommentId = command.CreateVoteDto.CommentId,
            UserId = command.CurrentUserId
        };

        await _repository.CreateCommentVoteAsync(commentVote, cancellationToken);
        
        return Unit.Value;
    }
}