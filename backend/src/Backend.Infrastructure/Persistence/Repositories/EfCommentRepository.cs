using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Persistence.Repositories;

public class EfCommentRepository : ICommentInterface
{
    private readonly ApplicationDbContext _context;

    public EfCommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<Comment?> GetCommentByIdAsync(Guid id)
    {
        return await _context.Comments
            .Include(c => c.Author) 
            .SingleOrDefaultAsync(comment => comment.Id == id);
    }

    public async Task<IEnumerable<Comment>> GetCommentsByContentIdAsync(int contentId)
    {
        return await _context.Comments
            .Include(c => c.Author) 
            .Where(comment => comment.ContentId == contentId)
            .ToListAsync();
    }

    public async Task<Comment> SaveCommentAsync(Comment comment)
    {
        if (comment.Id == Guid.Empty)
        {
            comment.Id = Guid.NewGuid();
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            await _context.Entry(comment).Reference(c => c.Author).LoadAsync();
            return comment;
        }
        else
        {
            var existing = await _context.Comments
                .Include(c => c.Author)
                .FirstOrDefaultAsync(c => c.Id == comment.Id);
            
            if (existing == null)
                throw new InvalidOperationException("Kommentar nicht gefunden");

            // Nur erlaubte Felder übernehmen (Text, ggf. ParentCommentId ...)
            existing.Text = comment.Text;
            // AuthorId sollte sich nie ändern – also nicht überschreiben!
            // existing.ParentCommentId = comment.ParentCommentId; (bei Bedarf)
            
            await _context.SaveChangesAsync();
            return existing; // Autor ist bereits durch Include gefüllt
        }
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