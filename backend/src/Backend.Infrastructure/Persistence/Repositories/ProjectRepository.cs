using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectInterface
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Project?> GetProjectByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Projects.SingleOrDefaultAsync(project => project.Id == id, cancellationToken);
        }

        public async Task<Project?> GetProjectBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Projects.SingleOrDefaultAsync(project => project.Slug == slug, cancellationToken);
        }

        public async Task<Project?> GetProjectDetailsBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Projects
                .Include(p => p.Author) 
                .Include(p => p.Content)
                .Include(p => p.Likes)      
                .Include(p => p.Comments)   
                .SingleOrDefaultAsync(project => project.Slug == slug, cancellationToken);
        }

        public async Task<Project> SaveProjectAsync(Project project, CancellationToken cancellationToken = default)
        {
            if (project.Id == 0)
            {
                _context.Projects.Add(project);
            }
            else
            {
                _context.Projects.Update(project);
            }
            await _context.SaveChangesAsync(cancellationToken);

            await _context.Entry(project).Reference(b => b.Author).LoadAsync(cancellationToken);
            return project;
        }

        public async Task DeleteProjectAsync(int id, CancellationToken cancellationToken = default)
        {
            var project = await _context.Projects.FindAsync(id, cancellationToken);

            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync(cancellationToken);
            }

        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync(CancellationToken cancellationToken = default)
        { 
            return await _context.Projects
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .OrderByDescending(project => project.DateOfCreation)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<Project> UpdateProjectViewsAsync(Project project, CancellationToken cancellationToken = default)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync(cancellationToken);
            return project;
        }



    }
}