namespace Backend.Application.Common.Interfaces;

public interface ICacheQueryQuery
{
    string CacheKey { get; }
    TimeSpan Expiration { get; }
}