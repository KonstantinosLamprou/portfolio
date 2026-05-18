using Backend.Domain.Interfaces;
using Backend.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.UseCases.SearchContent
{
    public class SearchProjectHandler
    {
        private readonly IProjectInterface _repository;
        public SearchProjectHandler(IProjectInterface repository)
        {
            _repository = repository;
        }

        public async Task<Project?> Handle(int ProjectId)
        {
            return await _repository.SearchProjectAsync(ProjectId); 
        }
    }
}
