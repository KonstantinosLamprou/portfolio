using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.UseCases.SearchContent
{
    public class SearchBlogHandler
    {
        private readonly IBlogInterface _repository;
        public SearchBlogHandler(IBlogInterface repository)
        {
            _repository = repository;
        }

        public async Task<Blog?> Handle(int BlogId)
        {
            return await _repository.SearchBlogAsync(BlogId);
        }
    }
}
