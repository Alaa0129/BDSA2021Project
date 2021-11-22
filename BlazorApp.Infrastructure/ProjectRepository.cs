using System;
using BlazorApp.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using static System.Net.HttpStatusCode;

namespace BlazorApp.Infrastructure
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly IPBankContext _context;

        public ProjectRepository(IPBankContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(ProjectCreateDTO project)
        {
            var entity = new Project
            {
                Title = project.Title,
                Description = project.Description,
                MaxApplications = project.MaxApplications,
                SupervisorId = project.SupervisorId
            };

            _context.Projects.Add(entity);

            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<ProjectDetailsDTO> ReadAsync(int projectId)
        {
            return await _context.Projects.Where(p => p.Id == projectId)
                                            .Select(p => new ProjectDetailsDTO
                                            (
                                                p.Id,
                                                p.Title,
                                                p.Description,
                                                p.SupervisorId,
                                                p.MaxApplications
                                            )).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<ProjectDetailsDTO>> ReadAsync()
        {
            return (await _context.Projects
                                    .Select(p => new ProjectDetailsDTO
                                    (
                                         p.Id,
                                        p.Title,
                                        p.Description,
                                        p.SupervisorId,
                                        p.MaxApplications
                                    )).ToListAsync()).AsReadOnly();
        }

        public async Task<HttpStatusCode> UpdateAsync(ProjectUpdateDTO project)
        {
            var entity = await _context.Projects.FindAsync(project.Id);

            if (entity == null) return NotFound;

            entity.Title = project.Title;
            entity.Description = project.Description;
            entity.MaxApplications = project.MaxApplications;
            entity.SupervisorId = project.SupervisorId;

            await _context.SaveChangesAsync();

            return OK;

        }
        public async Task<HttpStatusCode> DeleteAsync(int projectId)
        {
            var entity = await _context.Projects.FindAsync(projectId);

            if (entity == null) return NotFound;
            
            _context.Projects.Remove(entity);
            await _context.SaveChangesAsync();

            return OK;
        }
    }
}