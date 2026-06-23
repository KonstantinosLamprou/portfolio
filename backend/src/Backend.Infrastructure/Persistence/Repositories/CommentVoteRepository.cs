using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class CommentVoteRepository : ICommentVoteInterface
{
    private readonly ApplicationDbContext _context; 

    public CommentVoteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CommentVote>> GetCommentVotesAsync(Guid commentId)
    {
        return await _context.CommentVotes
            .Where(vote => vote.CommentId == commentId)
            .ToListAsync();
    }

    public async Task<CommentVote?> GetUserCommentVoteAsync(Guid commentId, Guid userId)
    {
        return await _context.CommentVotes
            .SingleOrDefaultAsync(vote => vote.CommentId == commentId && vote.UserId == userId);
    }

    public async Task CreateCommentVoteAsync(CommentVote vote)
    {
        
        _context.CommentVotes.Add(vote);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateCommentVoteAsync(bool isUpvote, Guid commentId, Guid userId)
    {
        var existingVote = await _context.CommentVotes
            .SingleOrDefaultAsync(vote => vote.CommentId == commentId && vote.UserId == userId);

        if (existingVote != null)
        {
            existingVote.IsUpvote = isUpvote;
            existingVote.VotedAt = DateTime.UtcNow;

            _context.CommentVotes.Update(existingVote);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> DeleteCommentVoteAsync(Guid commentId, Guid userId)
    {
        var existingVote = await _context.CommentVotes
            .SingleOrDefaultAsync(vote => vote.CommentId == commentId && vote.UserId == userId);

        if (existingVote != null)
        {
            _context.CommentVotes.Remove(existingVote);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

}