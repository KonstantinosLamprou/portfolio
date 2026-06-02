using Backend.Domain.Entities;

namespace Backend.Domain.Interfaces;

public interface ICommentInterface
{
    Task<Comment?> GetCommentByIdAsync(Guid id);
    Task<IEnumerable<Comment>> GetCommentsByContentIdAsync(int contentId);
    Task<Comment> SaveCommentAsync(Comment comment);
    Task<bool> DeleteCommentAsync(Guid id);
}