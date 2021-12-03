using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;

namespace BlazorApp.Core
{
    public interface ISupervisorRepository
    {
        Task<string> CreateAsync(SupervisorCreateDTO user);

        Task<SupervisorDetailsDTO> ReadAsync(string userId);

        Task<IReadOnlyCollection<SupervisorDTO>> ReadAsync();

        Task<HttpStatusCode> UpdateAsync(SupervisorUpdateDTO user);
    }
}