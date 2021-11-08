using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BlazorApp.Core;
using Microsoft.AspNetCore.Mvc;


namespace BlazorApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<UserDTO>> Get()
        {
            return await _repository.ReadAsync();
        }


        [HttpGet("{id}")]
        public async Task<UserDetailsDTO> Get(int id)
        {
            return await _repository.ReadAsync(id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UserUpdateDTO user)
        {
            var response = await _repository.UpdateAsync(user);

            if (response == HttpStatusCode.OK)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}