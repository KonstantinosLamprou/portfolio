using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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


        public async Task<Project?> SearchProjectAsync(int id)
        {
            return await _context.Projects.SingleOrDefaultAsync(project => project.Id == id);
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
            return await _context.Projects.ToListAsync();
        }


    }
}