using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BlazorApp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static System.Net.HttpStatusCode;

namespace BlazorApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestRepository _repository;
        private readonly ILogger<RequestController> _logger;

         public RequestController(ILogger<RequestController> logger ,IRequestRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }
       /* 
        This method returns every entry in the repository with requests through a GET request. 
        */
        [HttpGet("all")]
        public async Task<IEnumerable<RequestDTO>> Get()
        {
            return await _repository.ReadAsync();
        }
        /*
        This method returns a specific request in the repository using an id through a GET request
        */
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestDetailsDTO>> Get(int id)
        {
            var request =  await _repository.ReadAsync(id);

            if (request == null) return NotFound();

            return request;
        }
        /*
        This method is used when to create a request with a HTTP POST request. 
        */
        [HttpPost]
        public async Task<int> Post([FromBody] RequestCreateDTO request)
        {
            return await _repository.CreateAsync(request);
        }
        /*
        This method deletes requests through a DELETE request.
        */
        [HttpDelete("{id}")]
        public async Task<HttpStatusCode> Delete(int id)
        {
            var response = await _repository.DeleteAsync(id);

            if (response == OK)
            {
                return OK;
            }

            return HttpStatusCode.NotFound;
        }

    }
}