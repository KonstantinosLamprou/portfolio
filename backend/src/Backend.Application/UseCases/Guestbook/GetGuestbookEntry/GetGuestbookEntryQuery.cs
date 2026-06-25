using Backend.Application.Common.Interfaces;

public record GetGuestbookEntryQuery(Guid EntryId) : ICacheQueryQuery
{
    public string CacheKey => $"GuestbookEntry_{EntryId}";
    
    public TimeSpan Expiration => TimeSpan.FromMinutes(1);
}