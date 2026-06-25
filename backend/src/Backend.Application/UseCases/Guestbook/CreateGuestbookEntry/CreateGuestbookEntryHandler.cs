using Backend.Domain.Contracts;
using Backend.Domain.Interfaces;
using Backend.Domain.Entities;

namespace Backend.Application.UseCases.Guestbook;

public class CreateGuestbookEntryHandler
{
    private readonly IGuestbookEntry _guestbookEntryRepository;

    public CreateGuestbookEntryHandler(IGuestbookEntry guestbookEntryRepository)
    {
        _guestbookEntryRepository = guestbookEntryRepository;
    }

    public async Task<UserGuestbookEntryResponse> Handle(CreateGuestbookEntryRequest request, Guid UserId)
    {
        var newEntry = new GuestbookEntry
        {
            Id = Guid.Empty,
            Message = request.Message,
            CreatedAt = DateTime.UtcNow, 
            UserId = UserId
        };

        var savedEntry = await _guestbookEntryRepository.SaveEntryAsync(newEntry);

        return new UserGuestbookEntryResponse
        (
            savedEntry.Id,
            savedEntry.Message,
            savedEntry.CreatedAt,
            new AuthorDto(savedEntry.User.Id, savedEntry.User.Name, savedEntry.User.ProfilePictureUrl, savedEntry.User.Role)
        );
    }
}
