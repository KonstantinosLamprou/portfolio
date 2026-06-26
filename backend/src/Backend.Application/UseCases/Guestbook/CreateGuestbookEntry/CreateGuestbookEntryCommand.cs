using Backend.Application.Common.Interfaces;
using Backend.Domain.Contracts;

namespace Backend.Application.UseCases.Guestbook; 
public record CreateGuestbookEntryCommand(CreateGuestbookEntryRequest Request, Guid AuthorId) : ICacheInvalidatorCommand
{
    public IEnumerable<string> CacheKeysToInvalidate => new List<string>
    {
        // Invaliedieren des Caches für die Gästebucheinträge, da ein neuer Eintrag hinzugefügt wurde
        $"GuestbookEntries",
    };
}