using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Persistence.Repositories;

public class EfLikeRepository : ILikeInterface
{
    private readonly ApplicationDbContext _context;

    public EfLikeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Like?> GetLikeAsync(int contentId, Guid userId)
    {
        return await _context.Set<Like>()
            .SingleOrDefaultAsync(cl => cl.ContentId == contentId && cl.UserId == userId);
    }

    public void AddLike(Like like)
    {
        _context.Set<Like>().Add(like);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}


