using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorApp.Core;

namespace BlazorApp
{
    public class TagRemote : ITagRemote
    {
        private readonly HttpClient _httpClient;

        public TagRemote(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("default_client");
        }

        public async Task<IEnumerable<TagDetailsDTO>> GetTags()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<TagDetailsDTO>>("tag/all");
        }

    }
}