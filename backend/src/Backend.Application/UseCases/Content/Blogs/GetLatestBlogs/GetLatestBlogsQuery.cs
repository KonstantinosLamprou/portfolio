using Backend.Application.Common.Interfaces;

public record GetLatestBlogsQuery(int Count = 3) : ICacheQueryQuery
{
    public string CacheKey => $"Latest_Blogs";
    
    public TimeSpan Expiration => TimeSpan.FromMinutes(5);
}