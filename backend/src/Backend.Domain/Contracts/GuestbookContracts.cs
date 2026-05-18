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


public record CreateGuestbookentryRequest(
      string Message
    ); 