using Backend.Domain.Contracts;
using Backend.Domain.Interfaces;
using Backend.Application.Common.Interfaces;
namespace Backend.Application.UseCases.Guestbook;

public class GetGuestbookEntriesHandler : IQueryHandler<GetGuestbookEntriesQuery, IEnumerable<UserGuestbookEntryResponse>?>
{
    private readonly IGuestbookEntry _guestbookEntriesRepository;

    public GetGuestbookEntriesHandler(IGuestbookEntry guestbookEntriesRepository)
    {
        _guestbookEntriesRepository = guestbookEntriesRepository;
    }

    public async Task<IEnumerable<UserGuestbookEntryResponse>?> HandleAsync(GetGuestbookEntriesQuery query, CancellationToken cancellationToken = default)
    {
        var entries = await _guestbookEntriesRepository.GetAllEntriesAsync(cancellationToken);

        if (entries == null || !entries.Any())
            return null;

        return entries.Select(entry => new UserGuestbookEntryResponse
        (
            entry.Id,
            entry.Message,
            entry.CreatedAt,
            new AuthorDto(entry.User.Id, entry.User.Name, entry.User.ProfilePictureUrl, entry.User.Role)
        ));
    }
}