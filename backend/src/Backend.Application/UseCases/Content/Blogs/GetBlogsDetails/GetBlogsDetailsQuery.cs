using Backend.Application.Common.Interfaces;

public record GetBlogsDetailsQuery(string Slug, Guid? CurrentUserId) : ICacheQueryQuery
{
    public string CacheKey => $"BlogDetails_{Slug}";
    
    public TimeSpan Expiration => TimeSpan.FromMinutes(1);
}