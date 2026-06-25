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

    public async Task<IEnumerable<CommentVote>> GetCommentVotesAsync(Guid commentId, CancellationToken cancellationToken = default)
    {
        return await _context.CommentVotes
            .Where(vote => vote.CommentId == commentId)
            .ToListAsync(cancellationToken);
    }

    public async Task<CommentVote?> GetUserCommentVoteAsync(Guid commentId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.CommentVotes
            .SingleOrDefaultAsync(vote => vote.CommentId == commentId && vote.UserId == userId, cancellationToken);
    }

    public async Task CreateCommentVoteAsync(CommentVote vote, CancellationToken cancellationToken = default)
    {
        
        _context.CommentVotes.Add(vote);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateCommentVoteAsync(bool isUpvote, Guid commentId, Guid userId, CancellationToken cancellationToken = default)
    {
        var existingVote = await _context.CommentVotes
            .SingleOrDefaultAsync(vote => vote.CommentId == commentId && vote.UserId == userId, cancellationToken);

        if (existingVote != null)
        {
            existingVote.IsUpvote = isUpvote;
            existingVote.VotedAt = DateTime.UtcNow;

            _context.CommentVotes.Update(existingVote);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task DeleteCommentVoteAsync(Guid commentId, Guid userId, CancellationToken cancellationToken = default)
    {
        var existingVote = await _context.CommentVotes
            .SingleOrDefaultAsync(vote => vote.CommentId == commentId && vote.UserId == userId, cancellationToken);

        if (existingVote != null)
        {
            _context.CommentVotes.Remove(existingVote);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

}