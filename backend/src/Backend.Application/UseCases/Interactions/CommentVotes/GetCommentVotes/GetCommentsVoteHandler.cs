using Backend.Domain.Entities;
using Backend.Domain.Interfaces;

namespace Backend.Application.UseCases.Interactions;

public class GetCommentsVoteHandler
{
    private readonly ICommentVoteInterface _repository;

    public GetCommentsVoteHandler(ICommentVoteInterface repository)
    {
        _repository = repository;
    }

    public async Task<Dictionary<string, int>> Handle(Guid commentId)
    {   
        Dictionary<string, int> result = new Dictionary<string, int>();

        var existingVote = await _repository.GetCommentVotesAsync(commentId);

        result.Add("upvotes", existingVote.Count(c => c.CommentId == commentId && c.IsUpvote == true));
        result.Add("downvotes", existingVote.Count(c => c.CommentId == commentId && c.IsUpvote == false));

        return result;
    }
}