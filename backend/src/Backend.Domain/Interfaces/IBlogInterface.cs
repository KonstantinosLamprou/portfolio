using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Domain.Interfaces
{
    public interface IBlogInterface
    {
        Task<Blog?> GetBlogByIdAsync(int id); 
        Task<Blog?> GetBlogWithDetailsBySlugAsync(string slug);
        Task<Blog> SaveBlogAsync(Blog blog);
        Task<bool> DeleteBlogAsync(int id);
        Task<IEnumerable<Blog>> GetAllBlogsAsync();
        Task<IEnumerable<Blog>> GetLatestBlogsAsync(int count = 3);

    }
}
