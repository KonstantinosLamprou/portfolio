using Backend.Application.Common.Interfaces;

public record DeleteGuestbookEntryCommand(Guid EntryId, Guid CurrentUserId) : ICacheInvalidatorCommand
{
    public IEnumerable<string> CacheKeysToInvalidate =>
    [
        $"GuestbookEntries", 
        $"GuestbookEntry_{EntryId}"
    ];
}