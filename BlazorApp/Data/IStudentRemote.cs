using BlazorApp.Core;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorApp
{
    public interface IStudentRemote
    {
        Task<bool> CreateStudent(StudentCreateDTO student);
        Task<StudentDetailsDTO> GetStudent(string Id);
        Task<StudentDTO[]> GetStudents();
        Task<HttpResponseMessage> UpdateStudent(StudentUpdateDTO student);

        Task<HttpStatusCode> UpdateProject(int projectId, string studentId);
    }
}