using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorApp.Core;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Infrastructure
{
    public class TagRepository : ITagRepository
    {

        private readonly IPBankContext _context;

        public TagRepository(IPBankContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<TagDetailsDTO>> ReadAsyncAll()
        {
            return (await _context.Tags.Select(t => new TagDetailsDTO
                                             (
                                               t.Id,
                                               t.Name,
                                               t.Projects.Select(p => new ProjectDetailsDTO
                                                (
                                                    p.Id,
                                                    p.Title,
                                                    p.Description,
                                                    new SupervisorDTO(p.Supervisor.Id, p.Supervisor.Name),
                                                    p.AppliedStudents == null ? null : p.AppliedStudents.Select(p => new StudentDTO(p.Id, p.Name)).ToList(),
                                                    p.Tags.Select(t => t.Name).ToList()
                                                )).ToList()
                                             )).ToListAsync()).AsReadOnly();
        }

    }
}