using Backend.Domain.Contracts;
using Backend.Domain.Interfaces;

namespace Backend.Application.UseCases.Guestbook;

public class UpdateGuestbookEntryHandler
{
    private readonly IGuestbookEntry _guestbookEntryRepository;

    public UpdateGuestbookEntryHandler(IGuestbookEntry guestbookEntryRepository)
    {
        _guestbookEntryRepository = guestbookEntryRepository;
    }

    public async Task<UserGuestbookEntryResponse> Handle(Guid id, UpdateGuestbookEntryRequest request, Guid UserId)
    {
        var entry = await _guestbookEntryRepository.GetEntryByIdAsync(id) 
            ?? throw new KeyNotFoundException($"Gästebucheintrag mit ID {id} nicht gefunden.");

        entry.Message = request.Message;

        var updatedEntry = await _guestbookEntryRepository.SaveEntryAsync(entry);

        return new UserGuestbookEntryResponse
        (
            updatedEntry.Id,
            updatedEntry.Message,
            updatedEntry.CreatedAt,
            new AuthorDto(updatedEntry.User.Id, updatedEntry.User.Name, updatedEntry.User.ProfilePictureUrl, updatedEntry.User.Role)
        );
    }
}
