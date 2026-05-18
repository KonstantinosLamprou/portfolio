using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Backend.Application.UseCases.GetContent
{
    public class GetLatestBlogsHandler
    {
        private readonly IBlogInterface _repository;

        public GetLatestBlogsHandler(IBlogInterface repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Blog>> Handle(int count = 3)
        {
            return await _repository.GetLatestBlogsAsync(count); 
        }
    }
}
