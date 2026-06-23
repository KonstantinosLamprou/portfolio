using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class CommentRepository : ICommentInterface
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<Comment?> GetCommentByIdAsync(Guid id)
    {
        return await _context.Comments
            .Include(c => c.Author) 
            .Include(c => c.Votes)
            .SingleOrDefaultAsync(comment => comment.Id == id);
    }

    public async Task<IEnumerable<Comment>> GetCommentsByContentIdAsync(int contentId)
    {
        return await _context.Comments
            .Include(c => c.Author) 
            .Include(c => c.Votes)
            .Where(comment => comment.ContentId == contentId)
            .ToListAsync();
    }

    public async Task AddCommentAsync(Comment comment)
    {
        if (comment.Id == Guid.Empty)
        {
            comment.Id = Guid.NewGuid();
        }
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
        await _context.Entry(comment).Reference(c => c.Author).LoadAsync();
    }

    public async Task UpdateCommentAsync(Comment comment)
    {
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteCommentAsync(Guid id)
    {
        var comment = await _context.Comments.FindAsync(id); 

        if (comment == null)
        {
            return false; 
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return true;
    }

}