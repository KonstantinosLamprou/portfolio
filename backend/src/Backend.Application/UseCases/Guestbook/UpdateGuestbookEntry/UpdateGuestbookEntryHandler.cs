using Backend.Domain.Contracts;
using Backend.Domain.Interfaces;
using Backend.Application.Common.Interfaces;

namespace Backend.Application.UseCases.Guestbook;

public class UpdateGuestbookEntryHandler : ICommandHandler<UpdateGuestbookEntryCommand, UserGuestbookEntryResponse>
{
    private readonly IGuestbookEntry _guestbookEntryRepository;

    public UpdateGuestbookEntryHandler(IGuestbookEntry guestbookEntryRepository)
    {
        _guestbookEntryRepository = guestbookEntryRepository;
    }

    public async Task<UserGuestbookEntryResponse> HandleAsync(UpdateGuestbookEntryCommand command, CancellationToken cancellationToken = default)
    {
        var entry = await _guestbookEntryRepository.GetEntryByIdAsync(command.EntryId, cancellationToken)
            ?? throw new KeyNotFoundException($"Gästebucheintrag mit ID {command.EntryId} nicht gefunden.");

        entry.Message = command.Message;

        var updatedEntry = await _guestbookEntryRepository.SaveEntryAsync(entry, cancellationToken);

        return new UserGuestbookEntryResponse
        (
            updatedEntry.Id,
            updatedEntry.Message,
            updatedEntry.CreatedAt,
            new AuthorDto(updatedEntry.User.Id, updatedEntry.User.Name, updatedEntry.User.ProfilePictureUrl, updatedEntry.User.Role)
        );
    }
}
