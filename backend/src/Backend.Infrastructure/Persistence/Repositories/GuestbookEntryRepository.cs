using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class GuestbookEntryRepository : IGuestbookEntry
{
    private readonly ApplicationDbContext _context; 

    public GuestbookEntryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GuestbookEntry>?> GetAllEntriesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.GuestbookEntries
            .Include(e => e.User)
            .ToListAsync(cancellationToken);
    }

    public async Task<GuestbookEntry?> GetEntryByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.GuestbookEntries
            .Include(e => e.User) 
            .SingleOrDefaultAsync(entry => entry.Id == id, cancellationToken); 
    }

    public async Task<GuestbookEntry> SaveEntryAsync(GuestbookEntry entry, CancellationToken cancellationToken = default)
    {
        if (entry.Id == Guid.Empty)
        {
            entry.Id = Guid.NewGuid();
            _context.GuestbookEntries.Add(entry);
        } else
        {
            _context.GuestbookEntries.Update(entry);
        }
        await _context.SaveChangesAsync(cancellationToken);

        await _context.Entry(entry).Reference(e => e.User).LoadAsync(cancellationToken);

        return entry;
    }

    public async Task DeleteEntryAsync(GuestbookEntry? entry, CancellationToken cancellationToken = default)
    {
        if (entry != null)
        {
            _context.GuestbookEntries.Remove(entry);
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}