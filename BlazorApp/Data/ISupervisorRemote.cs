using BlazorApp.Core;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorApp
{
    public interface ISupervisorRemote
    {
        Task<bool> CreateSupervisor(SupervisorCreateDTO Supervisor);
        Task<HttpResponseMessage> UpdateSupervisor(SupervisorUpdateDTO Supervisor);
        Task<SupervisorDetailsDTO> GetSupervisor(string Id);
        Task<SupervisorDTO[]> GetSupervisors();
    }
}