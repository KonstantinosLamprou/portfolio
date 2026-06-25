using Backend.Application.Common.Interfaces;

namespace Backend.Application.UseCases.Comments;

public record GetCommentByIdQuery(Guid CommentId, Guid? CurrentUserId) : ICacheQueryQuery
{
    public string CacheKey => $"Comment_{CommentId}_{CurrentUserId}";
    
    public TimeSpan Expiration => TimeSpan.FromMinutes(1);
}