using BlazorApp.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp
{

public interface IRequestRemote
{
    Task<bool> CreateRequest(RequestCreateDTO request);

    Task<RequestDetailsDTO> GetRequest(int Id);

    Task<IEnumerable<RequestDetailsDTO>> GetRequests();
}


}