using Backend.Application.Common.Interfaces;

public record GetCommentsVoteQuery(Guid CommentId, Guid CurrentUserId) : ICacheQueryQuery
{
    public string CacheKey => $"CommentVotes_{CommentId}_{CurrentUserId}";
    
    public TimeSpan Expiration => TimeSpan.FromMinutes(1);
}