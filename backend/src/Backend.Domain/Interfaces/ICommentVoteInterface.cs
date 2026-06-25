using Backend.Domain.Entities;

namespace Backend.Domain.Interfaces;

public interface ICommentVoteInterface
{
    Task<IEnumerable<CommentVote>> GetCommentVotesAsync(Guid commentId, CancellationToken cancellationToken = default);
    Task<CommentVote?> GetUserCommentVoteAsync(Guid commentId, Guid userId, CancellationToken cancellationToken = default);
    Task CreateCommentVoteAsync(CommentVote vote, CancellationToken cancellationToken = default);
    Task UpdateCommentVoteAsync(bool isUpvote, Guid commentId, Guid userId, CancellationToken cancellationToken = default);
    Task DeleteCommentVoteAsync(Guid commentId, Guid userId, CancellationToken cancellationToken = default);
}