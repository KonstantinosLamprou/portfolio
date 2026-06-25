using Backend.Application.Common.Interfaces;
using Backend.Domain.Contracts;

namespace Backend.Application.UseCases.Content;

// Das Command implementiert unser ICacheInvalidatorCommand für den Decorator
public record CreateContentCommand(
    string ContentType, // "blog" oder "project"
    string Title, 
    string Slug, 
    string ImgSrc,
    string Description,
    IEnumerable<ContentBlockDto> Content, 
    string[] Tags,
    Guid CurrentUserId
) : ICacheInvalidatorCommand
{
    // Invalidiert die Listen-Caches nach dem Erstellen
    public IEnumerable<string> CacheKeysToInvalidate => ContentType.ToLower() switch
        {
            "blog" => ["Blogs"],
            "project" => ["Projects"],
            _ => [] // Fallback: Nichts löschen, falls der Typ unbekannt ist
        };
}