using Backend.Application.Common.Interfaces;

public record AddLikeCommand(int ContentId, Guid CurrentUserId, string ContentType, string Slug) : ICacheInvalidatorCommand
{
     public IEnumerable<string> CacheKeysToInvalidate => ContentType.ToLower() switch
        {
            "blog" => ["Blogs", $"BlogDetails_{Slug}"],
            "project" => ["Projects", $"ProjectDetails_{Slug}"],
            _ => [] // Fallback: Nichts löschen, falls der Typ unbekannt ist
        };
}