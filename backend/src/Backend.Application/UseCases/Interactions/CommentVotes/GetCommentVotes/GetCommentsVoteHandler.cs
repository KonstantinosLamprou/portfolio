using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Backend.Application.Common.Interfaces;


namespace Backend.Application.UseCases.Interactions;

public class GetCommentsVoteHandler : IQueryHandler<GetCommentsVoteQuery, Dictionary<string, int>>
{
    private readonly ICommentVoteInterface _repository;

    public GetCommentsVoteHandler(ICommentVoteInterface repository)
    {
        _repository = repository;
    }

    public async Task<Dictionary<string, int>> HandleAsync(GetCommentsVoteQuery query, CancellationToken cancellationToken = default)
    {   
        Dictionary<string, int> result = new Dictionary<string, int>();

        var existingVote = await _repository.GetCommentVotesAsync(query.CommentId, cancellationToken);

        result.Add("upvotes", existingVote.Count(c => c.CommentId == query.CommentId && c.IsUpvote == true));
        result.Add("downvotes", existingVote.Count(c => c.CommentId == query.CommentId && c.IsUpvote == false));

        return result;
    }
}