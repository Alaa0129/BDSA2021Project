using System;
using BlazorApp.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BlazorApp.Infrastructure
{
    public class UserRepository : IUserRepository
    {

        private readonly IPBankContext _context;

        public UserRepository(IPBankContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(UserCreateDTO user)
        {
            var entity = new User
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname
            };

            _context.Users.Add(entity);

            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public Task<UserDetailsDTO> ReadAsync(int userId)
        {
            var users = from u in _context.Users
                        where u.Id == userId
                        select new UserDetailsDTO
                        (
                            u.Id,
                            u.Firstname,
                            u.Lastname
                        );

            return users.FirstOrDefaultAsync();


        }
    }
}