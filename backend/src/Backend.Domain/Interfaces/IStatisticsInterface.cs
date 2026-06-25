using Backend.Domain.Entities;

namespace Backend.Domain.Interfaces;

public interface IStatisticsInterface
{
    Task<Statistics?> GetStatisticsAsync(CancellationToken cancellationToken = default);
    Task UpdateStatisticsAsync(Statistics statistics, CancellationToken cancellationToken = default);
}