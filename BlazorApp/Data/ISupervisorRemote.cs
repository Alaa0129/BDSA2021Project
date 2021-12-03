using BlazorApp.Core;
using System.Net;
using System.Threading.Tasks;

namespace BlazorApp
{
    public interface ISupervisorRemote
    {
        Task<bool> CreateSupervisor(SupervisorCreateDTO Supervisor);
        Task<HttpStatusCode> UpdateSupervisor(SupervisorUpdateDTO Supervisor);
        Task<SupervisorDetailsDTO> GetSupervisor(string Id);
        Task<SupervisorDTO[]> GetSupervisors();
    }
}