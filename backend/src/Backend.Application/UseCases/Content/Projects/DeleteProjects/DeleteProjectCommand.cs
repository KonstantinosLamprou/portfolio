using Backend.Application.Common.Interfaces;

public record DeleteProjectCommand(int ProjectId, Guid CurrentUserId) : ICacheInvalidatorCommand
{
    public IEnumerable<string> CacheKeysToInvalidate =>
    [
        $"Projects" 
    ];
}