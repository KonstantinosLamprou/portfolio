using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Persistence.Repositories;

public class EfGuestbookEntryRepository : IGuestbookEntry
{
    private readonly ApplicationDbContext _context; 

    public EfGuestbookEntryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GuestbookEntry>?> GetAllEntriesAsync()
    {
        return await _context.GuestbookEntries
            .Include(e => e.User)
            .ToListAsync();
    }

    public async Task<GuestbookEntry?> GetEntryByIdAsync(Guid id)
    {
        return await _context.GuestbookEntries
            .Include(e => e.User) 
            .SingleOrDefaultAsync(entry => entry.Id == id); 
    }

    public async Task<GuestbookEntry> SaveEntryAsync(GuestbookEntry entry)
    {
        if (entry.Id == Guid.Empty)
        {
            entry.Id = Guid.NewGuid();
            _context.GuestbookEntries.Add(entry);
        } else
        {
            _context.GuestbookEntries.Update(entry);
        }
        await _context.SaveChangesAsync();

        await _context.Entry(entry).Reference(e => e.User).LoadAsync();

        return entry;
    }

    public async Task<bool> DeleteEntryAsync(GuestbookEntry? entry)
    {
        if (entry != null)
        {
            _context.GuestbookEntries.Remove(entry);
            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }
}