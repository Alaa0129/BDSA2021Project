using System;
using BlazorApp.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Net;

namespace BlazorApp.Infrastructure
{
    public class SupervisorRepository : ISupervisorRepository
    {

        private readonly IPBankContext _context;

        public SupervisorRepository(IPBankContext context)
        {
            _context = context;
        }

        public async Task<string> CreateAsync(SupervisorCreateDTO Supervisor)
        {
            var entity = new Supervisor
            {
                Id = Supervisor.Id,
                Name = Supervisor.Name
            };

            _context.Supervisors.Add(entity);

            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<SupervisorDetailsDTO> ReadAsync(string SupervisorId)
        {
            var Supervisors = from s in _context.Supervisors
                           where s.Id == SupervisorId
                           select new SupervisorDetailsDTO
                           (
                               s.Id,
                               s.Name,
                               s.projects.Select(c => new ProjectDetailsDTO(c.Id, c.Title, c.Description, c.SupervisorId, c.MaxApplications)).ToList()
                           );

            return await Supervisors.FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<SupervisorDTO>> ReadAsync() =>
            (await _context.Supervisors
                            .Select(u => new SupervisorDTO(u.Id, u.Name))
                            .ToListAsync())
                            .AsReadOnly();

        public async Task<HttpStatusCode> UpdateAsync(SupervisorUpdateDTO Supervisor)
        {
            var entity = await _context.Supervisors.Where(u => u.Id == Supervisor.Id).FirstOrDefaultAsync();

            if (entity == null) return HttpStatusCode.NotFound;

            entity.Name = Supervisor.Name;

            await _context.SaveChangesAsync();

            return HttpStatusCode.OK;
        }
    }
}