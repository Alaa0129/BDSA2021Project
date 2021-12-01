using System.Threading.Tasks;
using BlazorApp.Core;
using System.Net.Http.Json;
using System;
using System.Net;
using System.Linq;
using System.Net.Http;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace BlazorApp
{

    public static class ProjectRemoteExtensions
    {
        public static void AddProjectRemote(this IServiceCollection services, IConfiguration configuration)
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            services.AddHttpClient<ProjectRemote>();
        }
    }
    public class ProjectRemote : IProjectRemote
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly string _APIScope = string.Empty;
        private readonly string _APIBaseAddress = string.Empty;

        public ProjectRemote(ITokenAcquisition tokenAcquisition, HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _tokenAcquisition = tokenAcquisition;
            _APIScope = configuration["API:APIScope"];
            _APIBaseAddress = configuration["API:APIBaseAddress"];
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
            return await _httpClient.GetFromJsonAsync<ProjectDetailsDTO>($"{_APIBaseAddress}/api/project/{Id}");
        }

        public async Task<ProjectDetailsDTO[]> GetProjects()
        {
            return await _httpClient.GetFromJsonAsync<ProjectDetailsDTO[]>($"{_APIBaseAddress}/api/project/all");
        }

        public async Task<HttpStatusCode> UpdateProject(ProjectUpdateDTO project)
        {
            await PrepareAuthenticatedClient();
            var result = await _httpClient.PutAsJsonAsync($"{_APIBaseAddress}/api/project/update", project);
            return result.StatusCode;
        }

        /// <summary>
        /// Retrieves the Access Token for the Web API.
        /// Sets Authorization and Accept headers for the request.
        /// </summary>
        /// <returns></returns>
        private async Task PrepareAuthenticatedClient()
        {
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { _APIScope });
            Console.WriteLine($"access token-{accessToken}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

    }
}