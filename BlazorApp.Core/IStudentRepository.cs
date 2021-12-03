using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;

namespace BlazorApp.Core
{
    public interface IStudentRepository
    {
        Task<string> CreateAsync(StudentCreateDTO user);

        Task<StudentDetailsDTO> ReadAsync(string userId);

        Task<IReadOnlyCollection<StudentDTO>> ReadAsync();

        Task<HttpStatusCode> UpdateAsync(StudentUpdateDTO user);
    }
}