using Backend.Application.Common.Interfaces;

public record UpdateBlogViewsCommand(int BlogId, string Slug) : ICacheInvalidatorCommand
{
    public IEnumerable<string> CacheKeysToInvalidate =>
    [
        $"Content_{Slug}",
        $"Content"
    ];
}