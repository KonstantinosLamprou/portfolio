using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Backend.Domain.Interfaces
{
    public interface IProjectInterface
    {
        Task<Project?> GetProjectByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Project?> GetProjectBySlugAsync(string slug, CancellationToken cancellationToken = default);
        Task<Project?> GetProjectDetailsBySlugAsync(string slug, CancellationToken cancellationToken = default);
        Task<Project> SaveProjectAsync(Project project, CancellationToken cancellationToken = default);
        Task DeleteProjectAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Project>> GetAllProjectsAsync(CancellationToken cancellationToken = default);
        Task<Project> UpdateProjectViewsAsync(Project project, CancellationToken cancellationToken = default);

    }
}
