using BlazorApp.Core;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BlazorApp
{
    public interface IProjectRemote
    {
        Task<bool> CreateProject(ProjectCreateDTO project);
        Task<HttpStatusCode> UpdateProject(ProjectUpdateDTO project);
        Task<ProjectDetailsDTO> GetProject(int Id);
        Task<IEnumerable<ProjectDetailsDTO>> GetProjects();

    }
}