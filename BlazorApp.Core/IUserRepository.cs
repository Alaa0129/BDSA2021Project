using System;
using System.Threading.Tasks;

namespace BlazorApp.Core
{
    public interface IUserRepository
    {
        Task<int> CreateAsync(UserCreateDTO user);

        Task<UserDetailsDTO> ReadAsync(int userId);
    }
}