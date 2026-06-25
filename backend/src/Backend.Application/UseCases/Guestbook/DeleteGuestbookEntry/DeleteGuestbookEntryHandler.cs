using Backend.Application.Common.Models;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Contracts;
using Backend.Domain.Interfaces;


namespace Backend.Application.UseCases.Guestbook;

public class DeleteGuestbookEntryHandler : ICommandHandler<DeleteGuestbookEntryCommand, Unit>
{
    private readonly IGuestbookEntry _guestbookEntryRepository;

    public DeleteGuestbookEntryHandler(IGuestbookEntry guestbookEntryRepository)
    {
        _guestbookEntryRepository = guestbookEntryRepository;
    }

    public async Task<Unit> HandleAsync(DeleteGuestbookEntryCommand command, CancellationToken cancellationToken = default)
    {
        var entry = await _guestbookEntryRepository.GetEntryByIdAsync(command.EntryId, cancellationToken) ?? throw new KeyNotFoundException($"Gästebucheintrag mit ID {command.EntryId} nicht gefunden.");
        
        if (entry?.UserId != command.CurrentUserId /* && !IsCurrentUserAdmin() */) 
            throw new UnauthorizedAccessException("Sie haben keine Berechtigung, diesen Gästebucheintrag zu löschen.");

        await _guestbookEntryRepository.DeleteEntryAsync(entry, cancellationToken);

        return Unit.Value;
    }
}