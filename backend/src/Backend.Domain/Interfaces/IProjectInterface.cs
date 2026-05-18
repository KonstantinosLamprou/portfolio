using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Domain.Interfaces
{
    public interface IProjectInterface
    {
        Task<Project?> SearchProjectAsync(int id);
        Task<Project> SaveProjectAsync(Project project);
        Task<bool> DeleteProjectAsync(int id);
        Task<IEnumerable<Project>> GetAllProjectsAsync();
    }
}
