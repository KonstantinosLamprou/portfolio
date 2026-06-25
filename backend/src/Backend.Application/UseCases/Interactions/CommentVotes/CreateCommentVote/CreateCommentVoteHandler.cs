using Backend.Domain.Contracts;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;

namespace Backend.Application.UseCases.Interactions;

public class CreateCommentVoteHandler
{
    private readonly ICommentVoteInterface _repository;

    public CreateCommentVoteHandler(ICommentVoteInterface repository)
    {
        _repository = repository;
    }

    public async Task Handle(CreateVoteDto createVoteDto, Guid userId)
    {
        var commentVote = new CommentVote
        {
            IsUpvote = createVoteDto.IsUpvote,
            CommentId = createVoteDto.CommentId,
            UserId = userId
        };

        await _repository.CreateCommentVoteAsync(commentVote);
    }
}