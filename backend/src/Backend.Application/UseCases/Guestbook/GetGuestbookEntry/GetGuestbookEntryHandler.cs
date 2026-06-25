using Backend.Domain.Contracts;
using Backend.Domain.Interfaces;
using Backend.Application.Common.Interfaces;

namespace Backend.Application.UseCases.Guestbook;

public class GetGuestbookEntryHandler : IQueryHandler<GetGuestbookEntryQuery, UserGuestbookEntryResponse>
{
    private readonly IGuestbookEntry _guestbookEntryRepository;

    public GetGuestbookEntryHandler(IGuestbookEntry guestbookEntryRepository)
    {
        _guestbookEntryRepository = guestbookEntryRepository;
    }

    public async Task<UserGuestbookEntryResponse> HandleAsync(GetGuestbookEntryQuery query, CancellationToken cancellationToken = default)
    {
        var entry = await _guestbookEntryRepository.GetEntryByIdAsync(query.EntryId, cancellationToken)
            ?? throw new KeyNotFoundException($"Gästebucheintrag mit ID {query.EntryId} nicht gefunden.");

        return new UserGuestbookEntryResponse
        (
            entry.Id,
            entry.Message,
            entry.CreatedAt,
            new AuthorDto(entry.User.Id, entry.User.Name, entry.User.ProfilePictureUrl, entry.User.Role)
        );
    }
}