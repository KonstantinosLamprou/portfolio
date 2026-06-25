using Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.UseCases.Content
{
    public class DeleteBlogHandler
    {
        private readonly IBlogInterface _repository;
        public DeleteBlogHandler(IBlogInterface repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(string slug, Guid currentUserId)
        {
            var blog = await _repository.GetBlogBySlugAsync(slug);

            if (blog == null)
                throw new KeyNotFoundException($"Blog mit Slug {slug} nicht gefunden.");

            if (blog.AuthorId != currentUserId /* && !IsCurrentUserAdmin() */) 
                throw new UnauthorizedAccessException("Sie haben keine Berechtigung, diesen Blog zu löschen.");

           return await _repository.DeleteBlogAsync(blog.Id); 
        }
    }
}
