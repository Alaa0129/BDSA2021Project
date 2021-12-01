using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BlazorApp.Core;
using Microsoft.AspNetCore.Mvc;
using static System.Net.HttpStatusCode;

namespace BlazorApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestRepository _repo;

         public RequestController(IRequestRepository repository)
        {
            _repo = repository;
        }
    
        [HttpGet("all")]
        public async Task<IEnumerable<RequestDetailsDTO>> Get()
        {
            return await _repo.ReadAsync();
        }
    
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestDetailsDTO>> Get(int id)
        {
            var request =  await _repo.ReadAsync(id);

            if (request == null) return NotFound();

            return request;
        }

        [HttpPost]
        public async Task<int> Post([FromBody] RequestCreateDTO request)
        {
            return await _repo.CreateAsync(request);
        }

        [HttpDelete("{id}")]
        public async Task<HttpStatusCode> Delete(int id)
        {
            var response = await _repo.DeleteAsync(id);

            if (response == OK)
            {
                return OK;
            }

            return HttpStatusCode.NotFound;
        }

    }
}