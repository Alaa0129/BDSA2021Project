using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorApp.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using System;

namespace BlazorApp
{

    public static class TagRemoteExtensions
    {
        public static void AddRequestRemote(this IServiceCollection services, IConfiguration configuration)
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            services.AddHttpClient<TagRemote>();
        }
    }
    public class TagRemote : ITagRemote
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly string _APIScope = string.Empty;
        private readonly string _APIBaseAddress = string.Empty;

        public TagRemote(ITokenAcquisition tokenAcquisition, HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _tokenAcquisition = tokenAcquisition;
            _APIScope = configuration["API:APIScope"];
            _APIBaseAddress = configuration["API:APIBaseAddress"];
        }

        public async Task<IEnumerable<TagDetailsDTO>> GetTags()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<TagDetailsDTO>>($"{_APIBaseAddress}/api/tag/all");
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