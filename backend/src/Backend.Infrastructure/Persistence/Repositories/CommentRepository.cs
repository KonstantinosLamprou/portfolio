using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading; 

namespace Backend.Infrastructure.Repositories;

public class CommentRepository : ICommentInterface
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Comment?> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .Include(c => c.Author) 
            .Include(c => c.Votes)
            .SingleOrDefaultAsync(comment => comment.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Comment>> GetCommentsByContentIdAsync(int contentId, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .Include(c => c.Author) 
            .Include(c => c.Votes)
            .Where(comment => comment.ContentId == contentId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddCommentAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        if (comment.Id == Guid.Empty)
        {
            comment.Id = Guid.NewGuid();
        }
        _context.Comments.Add(comment);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        await _context.Entry(comment).Reference(c => c.Author).LoadAsync(cancellationToken);
    }

    public async Task UpdateCommentAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        _context.Comments.Update(comment);
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}