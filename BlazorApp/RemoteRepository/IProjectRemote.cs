using BlazorApp.Core;
using System.Net;
using System.Threading.Tasks;

namespace BlazorApp
{
    public interface IProjectRemote
    {
        Task<bool> CreateProject(ProjectCreateDTO project);
        Task<HttpStatusCode> UpdateProject(ProjectUpdateDTO project);
        Task<ProjectDetailsDTO> GetProject(int Id);
        Task<ProjectDetailsDTO[]> GetProjects();

    }
}