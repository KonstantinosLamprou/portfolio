using Backend.Domain.Entities;

namespace Backend.Domain.Interfaces;

public interface IGuestbookEntry
{
    Task<IEnumerable<GuestbookEntry>?> GetAllEntriesAsync(CancellationToken cancellationToken = default);
    Task<GuestbookEntry?> GetEntryByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<GuestbookEntry> CreateEntryAsync(GuestbookEntry entry, CancellationToken cancellationToken = default);
    Task<GuestbookEntry> UpdateEntryAsync(GuestbookEntry entry, CancellationToken cancellationToken = default);
    Task DeleteEntryAsync(GuestbookEntry? entry, CancellationToken cancellationToken = default);
}