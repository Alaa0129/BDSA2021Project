using System.Threading.Tasks;
using BlazorApp.Core;
using System.Net.Http.Json;
using System;
using System.Net;
using System.Net.Http;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace BlazorApp
{

    public static class SupervisorRemoteExtensions
    {
        public static void AddSupervisorRemote(this IServiceCollection services, IConfiguration configuration)
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            services.AddHttpClient<SupervisorRemote>();
        }
    }
    public class SupervisorRemote : ISupervisorRemote
    {

        private readonly HttpClient _httpClient;
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly string _APIScope = string.Empty;
        private readonly string _APIBaseAddress = string.Empty;

        public SupervisorRemote(ITokenAcquisition tokenAcquisition, HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _tokenAcquisition = tokenAcquisition;
            _APIScope = configuration["API:APIScope"];
            _APIBaseAddress = configuration["API:APIBaseAddress"];
        }

        public async Task<bool> CreateSupervisor(SupervisorCreateDTO Supervisor)
        {
            var result = await _httpClient.PostAsJsonAsync($"{_APIBaseAddress}/api/Supervisor", Supervisor);
            var statusCode = (int)result.StatusCode;
            if (statusCode >= 200 && statusCode <= 208) return true;
            else return false;
        }

        public async Task<SupervisorDetailsDTO> GetSupervisor(string Id)
        {
            return await _httpClient.GetFromJsonAsync<SupervisorDetailsDTO>($"{_APIBaseAddress}/api/Supervisor/{Id}");
        }

        public async Task<SupervisorDTO[]> GetSupervisors()
        {
            return await _httpClient.GetFromJsonAsync<SupervisorDTO[]>($"{_APIBaseAddress}/api/Supervisor/all");
        }

        public async Task<HttpResponseMessage > UpdateSupervisor(SupervisorUpdateDTO Supervisor)
        {
            var result = await _httpClient.PutAsJsonAsync($"{_APIBaseAddress}/api/Supervisor/update", Supervisor);
            return result;
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