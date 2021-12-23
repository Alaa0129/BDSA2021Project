 using BlazorApp.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using static System.Net.HttpStatusCode;

namespace BlazorApp.Infrastructure
{
    public class RequestRepository : IRequestRepository
    {
        private readonly IPBankContext _context;

        public RequestRepository(IPBankContext context)
        {
            _context = context;
        }


        //Creates request and inserts it into the database
        public async Task<int> CreateAsync(RequestCreateDTO request)
        {
            var entity = new Request
            {
                Title = request.Title,
                Description = request.Description,
                Student = _context.Students.Find(request.StudentId),
                Supervisor = _context.Supervisors.Find(request.SupervisorId)
            };

            _context.Requests.Add(entity);

            await _context.SaveChangesAsync();

            return entity.Id;
        }

        //Gets a request by the specified requestId from the database
        public async Task<RequestDetailsDTO> ReadAsync(int requestId)
        {
            return await _context.Requests.Where(r => r.Id == requestId)
                                            .Select(r => new RequestDetailsDTO
                                            (
                                                r.Id,
                                                r.Title,
                                                r.Description,
                                                new StudentDTO(r.Student.Id, r.Student.Name),
                                                new SupervisorDTO(r.Supervisor.Id, r.Supervisor.Name)

                                            )).FirstOrDefaultAsync();
        }

        //Fetches all requests in the database
        public async Task<IReadOnlyCollection<RequestDTO>> ReadAsync()
        {
            return (await _context.Requests.Select(r => new RequestDTO
                                                       (
                                                           r.Id,
                                                           r.Title,
                                                           r.Description,
                                                           r.Student.Id,
                                                           r.Supervisor.Id
                                                       )).ToListAsync()).AsReadOnly();
        }

        //Deletes a request by the specified requestId
        public async Task<HttpStatusCode> DeleteAsync(int requestId)
        {
            var entity = await _context.Requests.FindAsync(requestId);

            if (entity == null) return NotFound;

            _context.Requests.Remove(entity);

            await _context.SaveChangesAsync();

            return OK;
        }
    }
}