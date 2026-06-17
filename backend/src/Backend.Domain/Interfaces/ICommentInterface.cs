using Backend.Domain.Entities;

namespace Backend.Domain.Interfaces;

public interface ICommentInterface
{
    Task<Comment?> GetCommentByIdAsync(Guid CommentId);
    Task<IEnumerable<Comment>> GetCommentsByContentIdAsync(int contentId);
    Task AddCommentAsync(Comment comment);
    Task UpdateCommentAsync(Comment comment);
    Task<bool> DeleteCommentAsync(Guid CommentId);
}