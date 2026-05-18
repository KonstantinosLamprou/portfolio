using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.UseCases.SaveContent
{
    public class SaveProjectHandler 
    {
        private readonly IProjectInterface _repository; 

        public SaveProjectHandler(IProjectInterface repository)
        {
            _repository = repository; 
        }


        public async Task<Project> Handle(Project project)
        {
           return await _repository.SaveProjectAsync(project);
        }
    }
}
