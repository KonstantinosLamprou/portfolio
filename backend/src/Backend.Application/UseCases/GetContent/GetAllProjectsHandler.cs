using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.UseCases.GetContent
{
    public class GetAllProjectsHandler
    {
        private readonly IProjectInterface _repository; 

        public GetAllProjectsHandler(IProjectInterface repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Project>> Handle()
        {
            return await _repository.GetAllProjectsAsync();
        }
    }
}
