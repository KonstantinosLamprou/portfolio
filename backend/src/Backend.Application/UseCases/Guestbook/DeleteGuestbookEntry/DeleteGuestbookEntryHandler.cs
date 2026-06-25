using Backend.Domain.Contracts;
using Backend.Domain.Interfaces;


namespace Backend.Application.UseCases.Guestbook;

public class DeleteGuestbookEntryHandler
{
    private readonly IGuestbookEntry _guestbookEntryRepository;

    public DeleteGuestbookEntryHandler(IGuestbookEntry guestbookEntryRepository)
    {
        _guestbookEntryRepository = guestbookEntryRepository;
    }

    public async Task<bool> Handle(Guid entryId, Guid currentUserId)
    {
        var entry = await _guestbookEntryRepository.GetEntryByIdAsync(entryId) ?? throw new KeyNotFoundException($"Gästebucheintrag mit ID {entryId} nicht gefunden.");
        
        if (entry?.UserId != currentUserId /* && !IsCurrentUserAdmin() */) 
            throw new UnauthorizedAccessException("Sie haben keine Berechtigung, diesen Gästebucheintrag zu löschen.");

        return await _guestbookEntryRepository.DeleteEntryAsync(entry);
    }
}