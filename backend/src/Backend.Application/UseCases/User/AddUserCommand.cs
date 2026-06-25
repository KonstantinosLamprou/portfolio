
namespace Backend.Application.UseCases.User;
public record AddUserCommand(
    string Provider,
    string ProviderSubjectId,
    string Email,
    string Name,
    string? ProfilePictureUrl
);
