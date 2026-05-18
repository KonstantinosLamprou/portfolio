using Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.UseCases.DeleteContent
{
    public class DeleteBlogHandler
    {
        private readonly IBlogInterface _repository;
        public DeleteBlogHandler(IBlogInterface repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(int BlogId)
        {
           return await _repository.DeleteBlogAsync(BlogId); 
        }
    }
}
