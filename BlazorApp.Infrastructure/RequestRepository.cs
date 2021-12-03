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

        public async Task<int> CreateAsync(RequestCreateDTO request)
        {
            var ent = new Request
                    {
                        Title = request.Title,
                        Description = request.Description,
                        StudentId = request.StudentId
                    };
            _context.Requests.Add(ent);
            await _context.SaveChangesAsync();
            return ent.Id;
        }

        public async Task<RequestDetailsDTO> ReadAsync(int requestId)
        {
            var request = _context.Requests.Where(r => r.Id == requestId)
                                            .Select(r => new RequestDetailsDTO
                                            (
                                                r.Id,
                                                r.Title,
                                                r.Description,
                                                r.StudentId
                                            )).FirstOrDefaultAsync();
            return await request;
        }

        public async Task<IReadOnlyCollection<RequestDetailsDTO>> ReadAsync()
        {
            var requests = _context.Requests.Select(r => new RequestDetailsDTO
                                                        (
                                                            r.Id,
                                                            r.Title,
                                                            r.Description,
                                                            r.StudentId
                                                        )).ToListAsync();
            var re = await requests;

            return re.AsReadOnly();
        }
    
        public async Task<HttpStatusCode> DeleteAsync(int requestId)
        {
            var ent = await _context.Requests.FindAsync(requestId);
            
            if (ent == null) return NotFound;

            _context.Requests.Remove(ent);

            await _context.SaveChangesAsync();

            return OK;
        }
    }
}