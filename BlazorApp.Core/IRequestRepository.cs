using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BlazorApp.Core
{
    public interface IRequestRepository
    {
        Task<int> CreateAsync(RequestCreateDTO request);
        Task<RequestDetailsDTO> ReadAsync(int requestId);
        Task<IReadOnlyCollection<RequestDetailsDTO>> ReadAsync();
        Task<HttpStatusCode> DeleteAsync(int requestId);
    }
}