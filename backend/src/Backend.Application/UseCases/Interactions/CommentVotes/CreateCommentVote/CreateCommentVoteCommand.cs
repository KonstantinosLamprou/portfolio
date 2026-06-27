using Backend.Application.Common.Interfaces;
using Backend.Domain.Contracts;

public record CreateCommentVoteCommand(CreateVoteDto CreateVoteDto, Guid CurrentUserId) : ICacheInvalidatorCommand
{
    public IEnumerable<string> CacheKeysToInvalidate =>
    [
        $"CommentVotes_{CreateVoteDto.CommentId}_{CurrentUserId}",
        $"CommentVotes_{CreateVoteDto.CommentId}_{Guid.Empty}",
        $"Comments_{CreateVoteDto.ContentId}_{CurrentUserId}",
        $"Comments_{CreateVoteDto.ContentId}_{Guid.Empty}" 
    ];
}