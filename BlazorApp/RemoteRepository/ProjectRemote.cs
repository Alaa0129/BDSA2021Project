using System.Threading.Tasks;
using BlazorApp.Core;
using System.Net.Http.Json;
using System;
using System.Net;
using System.Linq;
using System.Net.Http;

namespace BlazorApp
{
    public class ProjectRemote : IProjectRemote
    {
        private readonly HttpClient _httpClient;

        public ProjectRemote(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("default_client");
        }

        public async Task<bool> CreateProject(ProjectCreateDTO project)
        {
            var result = await _httpClient.PostAsJsonAsync("Project", project);
            var statusCode = (int)result.StatusCode;
            if (statusCode >= 200 && statusCode <= 208) return true;
            else return false;
        }

        public async Task<ProjectDetailsDTO> GetProject(int Id)
        {
            return await _httpClient.GetFromJsonAsync<ProjectDetailsDTO>($"project/{Id}");
        }

        public async Task<ProjectDetailsDTO[]> GetProjects()
        {
            return await _httpClient.GetFromJsonAsync<ProjectDetailsDTO[]>("project/all");
        }

        public async Task<HttpStatusCode> UpdateProject(ProjectUpdateDTO project)
        {
            var result = await _httpClient.PutAsJsonAsync("project/update", project);
            return result.StatusCode;
        }
    }
}