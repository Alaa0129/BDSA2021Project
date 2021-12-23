using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BlazorApp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace BlazorApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupervisorController : ControllerBase
    {

        private readonly ILogger<SupervisorController> _logger;
        private readonly ISupervisorRepository _repository;

        public SupervisorController(ILogger<SupervisorController> logger, ISupervisorRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }
               /* 
        This method returns every entry in the repository with supervisor through a GET request. 
        */
        [HttpGet("all")]
        public async Task<IEnumerable<SupervisorDTO>> Get()
        {
            return await _repository.ReadAsync();
        }
        /*
        This method returns a specific supervisor in the repository using an id through a GET request.
        */
        [HttpGet("{id}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult<SupervisorDetailsDTO>> Get(string id)
        {
            var Supervisor = await _repository.ReadAsync(id);

            if (Supervisor == null) return NotFound();

            else return Supervisor;
        }
        /*
        This method is used when to create a supervisor with a HTTP POST request. 
        */
        [HttpPost]
        public async Task<string> Post([FromBody] SupervisorCreateDTO Supervisor)
        {
            return await _repository.CreateAsync(Supervisor);
        }

        /* updates supervisor through a PUT request */
        [HttpPut("update")]
        public async Task<ActionResult> Put([FromBody] SupervisorUpdateDTO Supervisor)
        {
            var response = await _repository.UpdateAsync(Supervisor);

            if (response == HttpStatusCode.OK)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}