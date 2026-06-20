namespace Backend.Domain.Contracts;

public record AddUserCommand(
    string Provider,
    string ProviderSubjectId,
    string Email,
    string Name,
    string? ProfilePictureUrl
);

public record AddUserResult(
    Guid UserId,
    string Email,
    string Name,
    string Provider
);

public record UserDto(
    Guid UserId,
    string Email,
    string Name,
    string Provider, 
    string? ProfilePictureUrl

);