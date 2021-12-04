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
                Supervisor = _context.Supervisors.Find(project.SupervisorId),
                Tags = await GetTagsAsync(project.Tags).ToListAsync()
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
                                                new SupervisorDTO(p.Supervisor.Id, p.Supervisor.Name),
                                                p.AppliedStudents == null ? null : p.AppliedStudents.Select(p => new StudentDTO(p.Id, p.Name)).ToList(),
                                                p.Tags.Select(t => t.Name).ToList()
                                            )).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<ProjectDTO>> ReadAsync()
        {
            return (await _context.Projects
                                    .Select(p => new ProjectDTO
                                    (
                                            p.Id,
                                            p.Title,
                                            p.Description,
                                            p.Supervisor.Id
                                    )).ToListAsync()).AsReadOnly();
        }

        public async Task<HttpStatusCode> UpdateAsync(ProjectUpdateDTO project)
        {
            var entity = await _context.Projects.FindAsync(project.Id);

            if (entity == null) return NotFound;

            entity.Title = project.Title;
            entity.Description = project.Description;

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

        private async IAsyncEnumerable<Tag> GetTagsAsync(IEnumerable<string> tags)
        {
            var existing = await _context.Tags.Where(p => tags.Contains(p.Name)).ToDictionaryAsync(p => p.Name);

            foreach (var tag in tags)
            {
                yield return existing.TryGetValue(tag, out var p) ? p : new Tag(tag);
            }
        }
    }
}