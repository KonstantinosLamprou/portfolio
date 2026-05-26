using Backend.Domain.Entities;

namespace Backend.Domain.Interfaces;

public interface IApplicationUserRepository
{
    Task<ApplicationUser?> FindByProviderAsync(string provider, string providerSubjectId, CancellationToken ct);
    Task AddAsync(ApplicationUser user, CancellationToken ct);
    Task UpdateAsync(ApplicationUser user, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}