using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Domain.Contracts;

public record UserGuestbookEntryResponse(
    Guid Id,
    string Message,
    DateTime CreatedAt,
    AuthorDto Author
);

public record CreateGuestbookEntryRequest(
    string Message
);

public record UpdateGuestbookEntryRequest(
    string Message
);

public record GuestbookentryRequest(
    Guid Id,
    string Message
); 
