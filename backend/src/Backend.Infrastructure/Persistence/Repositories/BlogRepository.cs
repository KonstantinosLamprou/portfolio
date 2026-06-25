using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//refactoring 


namespace Backend.Infrastructure.Repositories
{
    public class BlogRepository : IBlogInterface
    {
        private readonly ApplicationDbContext _context; 

        public BlogRepository(ApplicationDbContext context)
        {
            _context = context; 
        }
        
        public async Task<Blog?> GetBlogByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Blogs.SingleOrDefaultAsync(blog => blog.Id == id, ct); 
        }

        public async Task<Blog?> GetBlogBySlugAsync(string slug, CancellationToken ct = default)
        {
            return await _context.Blogs.SingleOrDefaultAsync(blog => blog.Slug == slug, ct);
        }
        public async Task<Blog?> GetBlogWithDetailsBySlugAsync(string slug, CancellationToken ct = default)
        {
            return await _context.Blogs
                .Include(b => b.Author) 
                .Include(b => b.Content)
                .Include(b => b.Likes)      
                .Include(b => b.Comments)   
                .SingleOrDefaultAsync(blog => blog.Slug == slug, ct);
        }
        public async Task<Blog> SaveBlogAsync(Blog blog, CancellationToken ct = default)
        {
            if (blog.Id == 0)
            {
                _context.Blogs.Add(blog);
            }
            else
            {
                _context.Blogs.Update(blog);
            }
            await _context.SaveChangesAsync(ct);

            // Author laden für die Response!
            await _context.Entry(blog).Reference(b => b.Author).LoadAsync(ct);
            return blog;
        }

        public async Task DeleteBlogAsync(int id, CancellationToken ct = default)
        {
            var blog = await _context.Blogs.FindAsync(id, ct);

            if (blog != null)
            {
                _context.Blogs.Remove(blog);
                await _context.SaveChangesAsync(ct);
            }
        }
        public async Task<IEnumerable<Blog>> GetAllBlogsAsync(CancellationToken ct = default)
        {
            return await _context.Blogs
                .Include(b => b.Likes)
                .Include(b => b.Comments)
                .ToListAsync(ct);
        }
        public async  Task<IEnumerable<Blog>> GetLatestBlogsAsync(int count = 3, CancellationToken ct = default)
        {
            return await _context.Blogs
                .Include(b => b.Likes)
                .Include(b => b.Comments)
                .OrderByDescending(blog => blog.DateOfCreation)
                .Take(count)
                .ToListAsync(ct);
        }

        public async Task<Blog> UpdateBlogViewsAsync(Blog blog, CancellationToken ct = default)
        {
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync(ct);
            return blog;
        }


    }
}
