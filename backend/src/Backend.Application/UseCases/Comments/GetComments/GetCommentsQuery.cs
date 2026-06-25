using Backend.Application.Common.Interfaces;

namespace Backend.Application.UseCases.Comments;

public record GetCommentsQuery(int ContentId, string ContentType, Guid? CurrentUserId) : ICacheQueryQuery
{
    public string CacheKey => $"Comments_{ContentId}_{CurrentUserId}";
    
    public TimeSpan Expiration => TimeSpan.FromMinutes(1);
}