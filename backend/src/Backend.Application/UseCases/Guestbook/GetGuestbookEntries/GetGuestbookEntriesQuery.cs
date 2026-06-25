using Backend.Application.Common.Interfaces;

public record GetGuestbookEntriesQuery() : ICacheQueryQuery
{
    public string CacheKey => $"GuestbookEntries";
    
    public TimeSpan Expiration => TimeSpan.FromMinutes(1);
}