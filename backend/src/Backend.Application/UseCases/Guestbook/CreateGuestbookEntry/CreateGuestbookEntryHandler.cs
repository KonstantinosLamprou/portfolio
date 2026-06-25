using Backend.Domain.Contracts;
using Backend.Domain.Interfaces;
using Backend.Domain.Entities;
using Backend.Application.Common.Interfaces;

namespace Backend.Application.UseCases.Guestbook;

public class CreateGuestbookEntryHandler : ICommandHandler<CreateGuestbookEntryCommand, UserGuestbookEntryResponse>
{
    private readonly IGuestbookEntry _guestbookEntryRepository;

    public CreateGuestbookEntryHandler(IGuestbookEntry guestbookEntryRepository)
    {
        _guestbookEntryRepository = guestbookEntryRepository;
    }

    public async Task<UserGuestbookEntryResponse> HandleAsync(CreateGuestbookEntryCommand command, CancellationToken cancellationToken = default)
    {
        var newEntry = new GuestbookEntry
        {
            Id = Guid.Empty,
            Message = command.Request.Message,
            CreatedAt = DateTime.UtcNow, 
            UserId = command.AuthorId
        };

        var savedEntry = await _guestbookEntryRepository.SaveEntryAsync(newEntry, cancellationToken);

        return new UserGuestbookEntryResponse
        (
            savedEntry.Id,
            savedEntry.Message,
            savedEntry.CreatedAt,
            new AuthorDto(savedEntry.User.Id, savedEntry.User.Name, savedEntry.User.ProfilePictureUrl, savedEntry.User.Role)
        );
    }
}
