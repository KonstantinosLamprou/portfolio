using Backend.Application.Common.Interfaces;
public record GetAllBlogsQuery(Guid CurrentUserId) : ICacheQueryQuery
{
    public string CacheKey => $"Blogs";
    
    public TimeSpan Expiration => TimeSpan.FromMinutes(1);
}