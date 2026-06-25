using Backend.Domain.Contracts;
using Backend.Domain.Interfaces;

namespace Backend.Application.UseCases.Guestbook;

public class GetGuestbookEntryHandler
{
    private readonly IGuestbookEntry _guestbookEntryRepository;

    public GetGuestbookEntryHandler(IGuestbookEntry guestbookEntryRepository)
    {
        _guestbookEntryRepository = guestbookEntryRepository;
    }

    public async Task<UserGuestbookEntryResponse> Handle(Guid id)
    {
        var entry = await _guestbookEntryRepository.GetEntryByIdAsync(id) 
            ?? throw new KeyNotFoundException($"Gästebucheintrag mit ID {id} nicht gefunden.");

        return new UserGuestbookEntryResponse
        (
            entry.Id,
            entry.Message,
            entry.CreatedAt,
            new AuthorDto(entry.User.Id, entry.User.Name, entry.User.ProfilePictureUrl, entry.User.Role)
        );
    }
}