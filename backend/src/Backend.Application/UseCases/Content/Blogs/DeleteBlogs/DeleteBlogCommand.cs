using Backend.Application.Common.Interfaces;

public record DeleteBlogCommand(int BlogId, Guid CurrentUserId) : ICacheInvalidatorCommand
{
    public IEnumerable<string> CacheKeysToInvalidate =>
    [
        $"Blogs" 
    ];
}