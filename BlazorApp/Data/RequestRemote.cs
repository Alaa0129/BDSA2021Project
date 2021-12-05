using System.Threading.Tasks;
using BlazorApp.Core;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using System;

namespace BlazorApp
{
    public static class RequestRemoteExtensions
    {
        public static void AddRequestRemote(this IServiceCollection services, IConfiguration configuration)
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            services.AddHttpClient<RequestRemote>();
        }
    }

    public class RequestRemote : IRequestRemote
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly string _APIScope = string.Empty;
        private readonly string _APIBaseAddress = string.Empty;

        public RequestRemote(ITokenAcquisition tokenAcquisition, HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _tokenAcquisition = tokenAcquisition;
            _APIScope = configuration["API:APIScope"];
            _APIBaseAddress = configuration["API:APIBaseAddress"];
        }

        public async Task<bool> CreateRequest(RequestCreateDTO request)
        {
            var result = await _httpClient.PostAsJsonAsync($"{_APIBaseAddress}/api/request", request);
            var statusCode = (int)result.StatusCode;
            if (statusCode >= 200 && statusCode <= 208) return true;
            else return false;
        }


        public async Task<RequestDetailsDTO> GetRequest(int Id)
        {
            return await _httpClient.GetFromJsonAsync<RequestDetailsDTO>($"{_APIBaseAddress}/api/request/{Id}");
        }

        public async Task<IEnumerable<RequestDTO>> GetRequests()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<RequestDTO>>($"{_APIBaseAddress}/api/request/all");
        }
        public async Task<HttpStatusCode> DeleteRequest(int Id)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_APIBaseAddress}/api/request/delete", Id);
            return response.StatusCode;
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