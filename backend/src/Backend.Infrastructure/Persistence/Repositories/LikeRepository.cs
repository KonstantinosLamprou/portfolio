using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Repositories;

public class LikeRepository : ILikeInterface
{
    private readonly ApplicationDbContext _context;

    public LikeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Like?> GetLikeAsync(int contentId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Like>()
            .SingleOrDefaultAsync(cl => cl.ContentId == contentId && cl.UserId == userId, cancellationToken);
    }

    public void AddLike(Like like)
    {
        _context.Set<Like>().Add(like);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}


