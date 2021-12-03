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
                                                    p.SupervisorId,
                                                    p.MaxApplications,
                                                    p.Tags.Select(t => t.Name).ToList()
                                                )).ToList()
                                             )).ToListAsync()).AsReadOnly();
        }

    }
}