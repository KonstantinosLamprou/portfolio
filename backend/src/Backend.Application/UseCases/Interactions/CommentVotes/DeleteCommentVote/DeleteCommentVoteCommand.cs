using Backend.Application.Common.Interfaces;

public record DeleteCommentVoteCommand(Guid CommentId, int ContentId, Guid CurrentUserId) : ICacheInvalidatorCommand
{
    public IEnumerable<string> CacheKeysToInvalidate =>
    [
        $"CommentVotes_{CommentId}_{CurrentUserId}",
        $"CommentVotes_{CommentId}_{Guid.Empty}",
        $"Comments_{ContentId}_{CurrentUserId}", 
        $"Comments_{ContentId}_{Guid.Empty}" 

    ];
}