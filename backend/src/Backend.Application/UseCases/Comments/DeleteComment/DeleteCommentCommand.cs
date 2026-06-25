using Backend.Application.Common.Interfaces;
using Backend.Domain.Contracts;

namespace Backend.Application.UseCases.Comments;

public record DeleteCommentCommand(Guid CommentId, int ContentId, Guid CurrentUserId) : ICacheInvalidatorCommand
{
    public IEnumerable<string> CacheKeysToInvalidate =>
    [
        $"Comments_{ContentId}_{CurrentUserId}" 
    ];
}

