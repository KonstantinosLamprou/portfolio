using Backend.Domain.Entities;

namespace Backend.Domain.Interfaces;

public interface IGuestbookEntry
{
    Task<IEnumerable<GuestbookEntry>> GetAllEntriesAsync();
    Task<GuestbookEntry?> GetEntryByIdAsync(Guid id);
    Task<GuestbookEntry> SaveEntryAsync(GuestbookEntry entry);
    Task<bool>DeleteEntryAsync(GuestbookEntry? entry);
}