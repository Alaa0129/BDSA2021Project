using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;

namespace BlazorApp.Core
{
    public interface IProjectRepository
    {
        Task<int> CreateAsync(ProjectCreateDTO project);
        Task<ProjectDetailsDTO> ReadAsync(int projectId);
        Task<IReadOnlyCollection<ProjectDetailsDTO>> ReadAsync();
        Task<HttpStatusCode> UpdateAsync(ProjectUpdateDTO project);
        Task<HttpStatusCode> DeleteAsync(int projectId);
    }
}