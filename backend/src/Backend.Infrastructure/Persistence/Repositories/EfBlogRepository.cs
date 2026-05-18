using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Persistence.Repositories
{
    public class EfBlogRepository : IBlogInterface
    {
        private readonly ApplicationDbContext _context; 

        public EfBlogRepository(ApplicationDbContext context)
        {
            _context = context; 
        }
        

        public async Task<Blog?> SearchBlogAsync(int id)
        {
            return await _context.Blogs.SingleOrDefaultAsync(blog => blog.Id == id); 
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
            return await _context.Blogs.ToListAsync();
        }
        public async  Task<IEnumerable<Blog>> GetLatestBlogsAsync(int count = 3)
        {
            return await _context.Blogs
                .OrderByDescending(blog => blog.DateOfCreation)
                .Take(count)
                .ToListAsync();
        }


    }
}
