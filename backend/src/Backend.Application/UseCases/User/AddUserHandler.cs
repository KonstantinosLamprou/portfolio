using Backend.Domain.Contracts;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;

namespace Backend.Application.UseCases.User;

public class UpsertOAuthUserHandler
{
    private readonly IApplicationUserRepository _repo;

    public UpsertOAuthUserHandler(IApplicationUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<UpsertOAuthUserResult> Handle(UpsertOAuthUserCommand cmd, CancellationToken ct)
    {
        var user = await _repo.FindByProviderAsync(cmd.Provider, cmd.ProviderSubjectId, ct);

        if (user == null)
        {
            user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                AuthProvider = cmd.Provider,
                ProviderSubjectId = cmd.ProviderSubjectId,
                Email = cmd.Email,
                Name = cmd.Name,
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

        return new UpsertOAuthUserResult(user.Id, user.Email, user.Name, user.AuthProvider);
    }
}