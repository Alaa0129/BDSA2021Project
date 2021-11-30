using System.Threading.Tasks;
using BlazorApp.Core;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace BlazorApp
{
    public class RequestRemote : IRequestRemote
    {
        private readonly HttpClient _httpClient;

        public RequestRemote(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("default_client");
        }

        public async Task<bool> CreateRequest(RequestCreateDTO request)
        {
            var result = await _httpClient.PostAsJsonAsync("Request", request);
            var statusCode = (int) result.StatusCode;
            if (statusCode >= 200 && statusCode <= 208) return true;
            else return false;
        }

        public async Task<RequestDetailsDTO> GetRequest(int Id)
        {
            return await _httpClient.GetFromJsonAsync<RequestDetailsDTO>($"request/{Id}");
        }

        public async Task<IEnumerable<RequestDetailsDTO>> GetRequests()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<RequestDetailsDTO>>("request/all");
        }
    }
}