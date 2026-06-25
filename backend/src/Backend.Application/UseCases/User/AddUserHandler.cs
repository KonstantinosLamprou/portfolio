using Backend.Domain.Contracts;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Backend.Application.Options;
using Microsoft.Extensions.Options;
using Backend.Application.Common.Interfaces;

namespace Backend.Application.UseCases.User;

public class AddUserHandler : ICommandHandler<AddUserCommand, AddUserResult>
{
    private readonly IApplicationUserInterface _repo;
    private readonly HashSet<string> _adminEmails;

    public AddUserHandler(IApplicationUserInterface repo, IOptions<AdminOptions> adminOptions)
    {
        _repo = repo;
        _adminEmails = adminOptions.Value.Emails
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    public async Task<AddUserResult> HandleAsync(AddUserCommand cmd, CancellationToken ct)
    {
        var user = await _repo.FindByProviderAsync(cmd.Provider, cmd.ProviderSubjectId, ct);

        var isAdmin = _adminEmails.Contains(cmd.Email);


        if (user == null)
        {
            user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                AuthProvider = cmd.Provider,
                ProviderSubjectId = cmd.ProviderSubjectId,
                Email = cmd.Email,
                Name = cmd.Name,
                Role = isAdmin ? UserRole.Admin : UserRole.Standard, 
                ProfilePictureUrl = cmd.ProfilePictureUrl,
                LastLoginAt = DateTime.UtcNow
            };

            await _repo.AddAsync(user, ct);
        }
        else
        {
            user.LastLoginAt = DateTime.UtcNow;
            user.Name = cmd.Name;
            user.Email = cmd.Email;
            user.ProfilePictureUrl = cmd.ProfilePictureUrl;

            await _repo.UpdateAsync(user, ct);
        }

        await _repo.SaveChangesAsync(ct);

        return new AddUserResult(user.Id, user.Email, user.Name, user.AuthProvider, user.ProfilePictureUrl, user.Role);
    }
}