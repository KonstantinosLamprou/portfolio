using Backend.Domain.Entities;

namespace Backend.Domain.Interfaces;

public interface ICommentVoteInterface
{
    Task<IEnumerable<CommentVote>> GetCommentVotesAsync(Guid commentId);
    Task<CommentVote?> GetUserCommentVoteAsync(Guid commentId, Guid userId);
    Task CreateCommentVoteAsync(CommentVote vote);
    Task UpdateCommentVoteAsync(bool isUpvote, Guid commentId, Guid userId);
    Task<bool> DeleteCommentVoteAsync(Guid commentId, Guid userId);
}