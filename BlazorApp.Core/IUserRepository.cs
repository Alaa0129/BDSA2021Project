using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;

namespace BlazorApp.Core
{
    public interface IUserRepository
    {
        Task<int> CreateAsync(UserCreateDTO user);

        Task<UserDetailsDTO> ReadAsync(int userId);

        Task<IReadOnlyCollection<UserDTO>> ReadAsync();

        Task<HttpStatusCode> UpdateAsync(UserUpdateDTO user);
    }
}