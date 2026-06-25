using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Backend.Infrastructure.Repositories;

public class StatisticsRepository : IStatisticsInterface
{
    private readonly ApplicationDbContext _context;

    public StatisticsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Statistics?> GetStatisticsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Statistics.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateStatisticsAsync(Statistics statistics, CancellationToken cancellationToken = default)
    {
        _context.Statistics.Update(statistics);
        await _context.SaveChangesAsync(cancellationToken);
    }
}