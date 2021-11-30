using System;
using BlazorApp.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Net;

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

        public async Task<UserDetailsDTO> ReadAsync(int userId)
        {
            var users = from u in _context.Users
                        where u.Id == userId
                        select new UserDetailsDTO
                        (
                            u.Id,
                            u.Firstname,
                            u.Lastname
                        );

            return await users.FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<UserDTO>> ReadAsync() =>
            (await _context.Users
                            .Select(u => new UserDTO(u.Id, u.Firstname, u.Lastname))
                            .ToListAsync())
                            .AsReadOnly();

        public async Task<HttpStatusCode> UpdateAsync(UserUpdateDTO user)
        {
            var entity = await _context.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();

            if (entity == null) return HttpStatusCode.NotFound;

            entity.Firstname = user.Firstname;
            entity.Lastname = user.Lastname;

            await _context.SaveChangesAsync();

            return HttpStatusCode.OK;
        }
    }
}