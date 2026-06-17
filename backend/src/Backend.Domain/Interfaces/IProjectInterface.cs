using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Domain.Interfaces
{
    public interface IProjectInterface
    {
        Task<Project?> GetProjectByIdAsync(int id);

        Task<Project?> GetProjectBySlugAsync(string slug);

        Task<Project?> GetProjectDetailsBySlugAsync(string slug);
        Task<Project> SaveProjectAsync(Project project);
        Task<bool> DeleteProjectAsync(int id);
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<Project> UpdateProjectViewsAsync(Project project);

    }
}
