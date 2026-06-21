using Backend.Domain.Entities;

namespace Backend.Domain.Interfaces;

public interface IStatisticsInterface
{
    Task<Statistics?> GetStatisticsAsync();
    Task UpdateStatisticsAsync(Statistics statistics);
}