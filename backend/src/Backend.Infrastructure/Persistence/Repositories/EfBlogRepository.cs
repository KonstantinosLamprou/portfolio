using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//refactoring 


namespace Backend.Infrastructure.Persistence.Repositories
{
    public class EfBlogRepository : IBlogInterface
    {
        private readonly ApplicationDbContext _context; 

        public EfBlogRepository(ApplicationDbContext context)
        {
            _context = context; 
        }
        
        public async Task<Blog?> GetBlogByIdAsync(int id)
        {
            return await _context.Blogs.SingleOrDefaultAsync(blog => blog.Id == id); 
        }

        public async Task<Blog?> GetBlogBySlugAsync(string slug)
        {
            return await _context.Blogs.SingleOrDefaultAsync(blog => blog.Slug == slug);
        }
        public async Task<Blog?> GetBlogWithDetailsBySlugAsync(string slug)
        {
            return await _context.Blogs
                .Include(b => b.Author) 
                .Include(b => b.Content)
                .Include(b => b.Likes)      
                .Include(b => b.Comments)   
                .SingleOrDefaultAsync(blog => blog.Slug == slug);
        }
        public async Task<Blog> SaveBlogAsync(Blog blog)
        {
            if (blog.Id == 0)
            {
                _context.Blogs.Add(blog);
            }
            else
            {
                _context.Blogs.Update(blog);
            }
            await _context.SaveChangesAsync();

            // Author laden für die Response!
            await _context.Entry(blog).Reference(b => b.Author).LoadAsync();
            return blog;
        }

        public async Task<bool> DeleteBlogAsync(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);

            if (blog == null)
                return false;

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Blog>> GetAllBlogsAsync()
        {
            return await _context.Blogs
                .Include(b => b.Likes)
                .Include(b => b.Comments)
                .ToListAsync();
        }
        public async  Task<IEnumerable<Blog>> GetLatestBlogsAsync(int count = 3)
        {
            return await _context.Blogs
                .Include(b => b.Likes)
                .Include(b => b.Comments)
                .OrderByDescending(blog => blog.DateOfCreation)
                .Take(count)
                .ToListAsync();
        }

        public async Task<Blog> UpdateBlogViewsAsync(Blog blog)
        {
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
            return blog;
        }


    }
}
