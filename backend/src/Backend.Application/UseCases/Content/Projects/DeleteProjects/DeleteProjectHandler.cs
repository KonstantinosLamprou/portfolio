using Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.UseCases.Content
{
    public class DeleteProjectHandler
    {
        private readonly IProjectInterface _repository;

        public DeleteProjectHandler(IProjectInterface repository)
        {
            _repository = repository;
        }

        public async Task Handle(string slug, Guid currentUserId)
        {
            var project = await _repository.GetProjectBySlugAsync(slug);

            if (project == null)
                throw new KeyNotFoundException($"Projekt mit Slug {slug} nicht gefunden.");

            if (project.AuthorId != currentUserId /* && !IsCurrentUserAdmin() */)
                throw new UnauthorizedAccessException("Sie haben keine Berechtigung, dieses Projekt zu löschen.");

            await _repository.DeleteProjectAsync(project.Id);
        }
    }
}
