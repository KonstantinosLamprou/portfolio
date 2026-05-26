namespace Backend.Domain.Contracts;

public record UpsertOAuthUserCommand(
    string Provider,
    string ProviderSubjectId,
    string Email,
    string Name,
    string? ProfilePictureUrl
);

public record UpsertOAuthUserResult(
    Guid UserId,
    string Email,
    string Name,
    string Provider
);