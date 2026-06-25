using Backend.Application.Common.Interfaces;

public record DeleteCommentVoteCommand(Guid CommentId, Guid CurrentUserId) : ICacheInvalidatorCommand
{
    public IEnumerable<string> CacheKeysToInvalidate =>
    [
        $"CommentVotes_{CommentId}_{CurrentUserId}",
        $"Comments_{CommentId}_{CurrentUserId}"
    ];
}