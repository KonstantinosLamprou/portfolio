using Backend.Application.Common.Interfaces;

public record GetStatisticsQuery() : ICacheQueryQuery
{
    public string CacheKey => $"Statistics";
    
    public TimeSpan Expiration => TimeSpan.FromMinutes(1);
}