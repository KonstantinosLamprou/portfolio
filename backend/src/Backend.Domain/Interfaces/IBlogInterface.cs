using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Backend.Domain.Interfaces
{
    public interface IBlogInterface
    {
        Task<Blog?> GetBlogByIdAsync(int id, CancellationToken cancellationToken = default); 
        Task<Blog?> GetBlogBySlugAsync(string slug, CancellationToken cancellationToken = default);
        Task<Blog?> GetBlogWithDetailsBySlugAsync(string slug, CancellationToken cancellationToken = default);
        Task<Blog> SaveBlogAsync(Blog blog, CancellationToken cancellationToken = default);
        Task DeleteBlogAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Blog>> GetAllBlogsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Blog>> GetLatestBlogsAsync(int count = 3, CancellationToken cancellationToken = default);

        Task<Blog> UpdateBlogViewsAsync(Blog blog, CancellationToken cancellationToken = default);

    }
}
