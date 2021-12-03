using BlazorApp.Core;
using System.Net;
using System.Threading.Tasks;

namespace BlazorApp
{
    public interface IStudentRemote
    {
        Task<bool> CreateStudent(StudentCreateDTO student);
        Task<HttpStatusCode> UpdateStudent(StudentUpdateDTO student);
        Task<StudentDetailsDTO> GetStudent(string Id);
        Task<StudentDTO[]> GetStudents();
    }
}