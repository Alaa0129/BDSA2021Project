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

    public static class StudentRemoteExtensions
    {
        public static void AddStudentRemote(this IServiceCollection services, IConfiguration configuration)
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            services.AddHttpClient<StudentRemote>();
        }
    }
    public class StudentRemote : IStudentRemote
    {

        private readonly HttpClient _httpClient;
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly string _APIScope = string.Empty;
        private readonly string _APIBaseAddress = string.Empty;

        public StudentRemote(ITokenAcquisition tokenAcquisition, HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _tokenAcquisition = tokenAcquisition;
            _APIScope = configuration["API:APIScope"];
            _APIBaseAddress = configuration["API:APIBaseAddress"];
        }

        public async Task<bool> CreateStudent(StudentCreateDTO student)
        {
            var result = await _httpClient.PostAsJsonAsync($"{_APIBaseAddress}/api/student", student);
            var statusCode = (int)result.StatusCode;
            if (statusCode >= 200 && statusCode <= 208) return true;
            else return false;
        }

        public async Task<StudentDetailsDTO> GetStudent(string Id)
        {
            return await _httpClient.GetFromJsonAsync<StudentDetailsDTO>($"{_APIBaseAddress}/api/student/{Id}");
        }

        public async Task<StudentDTO[]> GetStudents()
        {
            return await _httpClient.GetFromJsonAsync<StudentDTO[]>($"{_APIBaseAddress}/api/Student/all");
        }

        public async Task<HttpStatusCode> UpdateStudent(StudentUpdateDTO student)
        {
            var result = await _httpClient.PutAsJsonAsync($"{_APIBaseAddress}/api/student/update", student);
            return result.StatusCode;
        }

        private async Task PrepareAuthenticatedClient()
        {
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { _APIScope });
            Console.WriteLine($"access token-{accessToken}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}