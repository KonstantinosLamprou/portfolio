using Backend.Application.Common.Interfaces;
using Backend.Domain.Contracts;

namespace Backend.Application.UseCases.Comments;

public record UpdateCommentCommand(Guid CommentId, int ContentId, UpdateCommentRequest Request, Guid CurrentUserId) : ICacheInvalidatorCommand
{
    public IEnumerable<string> CacheKeysToInvalidate =>
    [
        $"Comments_{ContentId}_{CurrentUserId}" 
    ];
}