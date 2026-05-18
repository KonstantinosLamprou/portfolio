using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.UseCases.GetContent
{
    public class GetAllBlogsHandler
    {
        private readonly IBlogInterface _repository; 

        public GetAllBlogsHandler(IBlogInterface repository)
        {
            _repository = repository; 
        }

        public async Task<IEnumerable<Blog>> Handle()
        {
            return await _repository.GetAllBlogsAsync(); 
        }
    }
}
