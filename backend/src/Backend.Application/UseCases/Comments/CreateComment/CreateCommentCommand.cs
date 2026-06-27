using Backend.Application.Common.Interfaces;
using Backend.Domain.Contracts;

namespace Backend.Application.UseCases.Comments;

public record CreateCommentCommand(CreateCommentRequest request, Guid authorId) : ICacheInvalidatorCommand
{
    public IEnumerable<string> CacheKeysToInvalidate => new List<string>
    {
        // Invaliedieren des Caches für die Kommentare des Contents, da ein neuer Kommentar hinzugefügt wurde
        // AuthorId ist die UserId aus dem ApplicationUser Modell
        $"Comments_{request.ContentId}_{authorId}",
        $"Comments_{request.ContentId}_{Guid.Empty}" 
    };
}