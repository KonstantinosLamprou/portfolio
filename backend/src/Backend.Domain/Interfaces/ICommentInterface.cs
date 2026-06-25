using Backend.Domain.Entities;

namespace Backend.Domain.Interfaces;

public interface ICommentInterface
{
    Task<Comment?> GetCommentByIdAsync(Guid commentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Comment>> GetCommentsByContentIdAsync(int contentId, CancellationToken cancellationToken = default);
    Task AddCommentAsync(Comment comment, CancellationToken cancellationToken = default);
    Task UpdateCommentAsync(Comment comment, CancellationToken cancellationToken = default);
}