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
        // Fehlt hier das Mapping von Entity zu DTO? Oder soll hier direkt die Entity zurückgegeben werden?
        public async Task<IEnumerable<Blog>> Handle(int count = 3)
        {
            return await _repository.GetLatestBlogsAsync(count); 
        }
    }
}
