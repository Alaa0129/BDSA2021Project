using BlazorApp.Core;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BlazorApp
{

    public interface IRequestRemote
    {
        Task<bool> CreateRequest(RequestCreateDTO request);

        Task<RequestDetailsDTO> GetRequest(int Id);

        Task<IEnumerable<RequestDTO>> GetRequests();

        Task<HttpStatusCode> DeleteRequest(int Id);
    }


}