using Backend.Domain.Entities;
namespace Backend.Domain.Contracts;

public record AddUserResult(
    Guid UserId,
    string Email,
    string Name,
    string Provider,
    string? ProfilePictureUrl,
    UserRole Role
);

public record UserDto(
    Guid UserId,
    string Email,
    string Name,
    string Provider, 
    string? ProfilePictureUrl,
    string Role
);

