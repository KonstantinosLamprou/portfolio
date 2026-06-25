using Backend.Application.Common.Interfaces;
public record GetAllProjectsQuery(Guid CurrentUserId) : ICacheQueryQuery
{
    public string CacheKey => $"Projects";
    
    public TimeSpan Expiration => TimeSpan.FromMinutes(1);
}