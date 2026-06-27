using Backend.Application.Common.Interfaces;

public record UpdateProjectViewsCommand(int ProjectId, string Slug) : ICacheInvalidatorCommand
{
    public IEnumerable<string> CacheKeysToInvalidate =>
    [
        $"ProjectDetails_{Slug}",
        $"Projects"
    ];
}
