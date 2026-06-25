using Backend.Application.Common.Interfaces;

public record UpdateGuestbookEntryCommand(Guid EntryId, string Message, Guid CurrentUserId) : ICacheInvalidatorCommand
{
    public IEnumerable<string> CacheKeysToInvalidate =>
    [
        $"GuestbookEntries", 
        $"GuestbookEntry_{EntryId}"
    ];
}