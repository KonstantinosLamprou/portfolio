using Backend.Application.Common.Interfaces;

public record UpdateCommentVoteCommand(Guid CommentId, bool IsUpvote, Guid CurrentUserId) : ICacheInvalidatorCommand
{
    public IEnumerable<string> CacheKeysToInvalidate =>
    [
        $"CommentVotes_{CommentId}_{CurrentUserId}",
        $"Comments_{CommentId}_{CurrentUserId}"
    ];
}