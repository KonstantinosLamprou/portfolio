using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Backend.Infrastructure.Persistence.Repositories;

public class EfStatisticsRepository : IStatisticsInterface
{
    private readonly ApplicationDbContext _context;

    public EfStatisticsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Statistics?> GetStatisticsAsync()
    {
        return await _context.Statistics.FirstOrDefaultAsync();
    }

    public async Task UpdateStatisticsAsync(Statistics statistics)
    {
        _context.Statistics.Update(statistics);
        await _context.SaveChangesAsync();
    }
}