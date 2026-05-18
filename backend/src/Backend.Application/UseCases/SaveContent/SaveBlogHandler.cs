using System;
using System.Collections.Generic;
using System.Text;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces; 


namespace Backend.Application.UseCases.SaveContent
{
    public class SaveBlogHandler
    {
        private readonly IBlogInterface _repository; 
        public SaveBlogHandler(IBlogInterface repository)
        {
            _repository = repository; 
        }

        public async Task<Blog> Handle(Blog blog)
        {
            return await _repository.SaveBlogAsync(blog); 
        }
    }
}
