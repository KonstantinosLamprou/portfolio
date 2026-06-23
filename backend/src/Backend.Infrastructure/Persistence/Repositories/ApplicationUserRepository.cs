using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class ApplicationUserRepository : IApplicationUserInterface
{
    private readonly ApplicationDbContext _context;

    public ApplicationUserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<ApplicationUser?> FindByProviderAsync(string provider, string providerSubjectId, CancellationToken ct)
    {
        return _context.Users
            .FirstOrDefaultAsync(u => u.AuthProvider == provider && u.ProviderSubjectId == providerSubjectId, ct);
    }

    public Task AddAsync(ApplicationUser user, CancellationToken ct)
    {
        _context.Users.Add(user);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(ApplicationUser user, CancellationToken ct)
    {
        _context.Users.Update(user);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct)
    {
        return _context.SaveChangesAsync(ct);
    }
}