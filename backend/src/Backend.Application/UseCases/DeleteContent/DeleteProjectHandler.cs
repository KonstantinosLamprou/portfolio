using Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.UseCases.DeleteContent
{
    public class DeleteProjectHandler
    {
        private readonly IProjectInterface _repository;

        public DeleteProjectHandler(IProjectInterface repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(int ProjectId)
        {
            return await _repository.DeleteProjectAsync(ProjectId); 
        }
    }
}
