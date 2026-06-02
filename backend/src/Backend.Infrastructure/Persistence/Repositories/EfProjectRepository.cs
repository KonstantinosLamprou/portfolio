using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Persistence.Repositories
{
    public class EfProjectRepository : IProjectInterface
    {
        private readonly ApplicationDbContext _context;

        public EfProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            return await _context.Projects.SingleOrDefaultAsync(project => project.Id == id);
        }

        public async Task<Project?> GetProjectWithDetailsAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .SingleOrDefaultAsync(project => project.Id == id); 

        }

        public async Task<Project?> GetProjectBySlugAsync(string slug)
        {
            return await _context.Projects
                .Include(p => p.Author) 
                .Include(p => p.Content)
                .Include(p => p.Likes)      
                .Include(p => p.Comments)   
                .SingleOrDefaultAsync(project => project.Slug == slug);
        }

        public async Task<Project> SaveProjectAsync(Project project)
        {
            if (project.Id == 0)
            {
                _context.Projects.Add(project);
            }
            else
            {
                _context.Projects.Update(project);
            }
            await _context.SaveChangesAsync();

            await _context.Entry(project).Reference(b => b.Author).LoadAsync();
            return project;
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
                return false;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        { 
            return await _context.Projects
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .OrderByDescending(project => project.DateOfCreation)
                .ToListAsync();
        }


    }
}