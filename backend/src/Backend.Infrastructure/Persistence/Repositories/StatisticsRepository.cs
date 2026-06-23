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