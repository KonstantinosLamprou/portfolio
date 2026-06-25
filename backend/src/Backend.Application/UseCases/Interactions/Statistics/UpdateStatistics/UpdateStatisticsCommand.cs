using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;

public record UpdateStatisticsCommand(int? ViewsToAdd, int? LikesToAdd) : ICacheInvalidatorCommand
{
    public IEnumerable<string> CacheKeysToInvalidate =>
    [
        $"Statistics"
    ];
}