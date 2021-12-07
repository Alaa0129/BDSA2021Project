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

        [HttpGet("all")]
        public async Task<IEnumerable<SupervisorDTO>> Get()
        {
            return await _repository.ReadAsync();
        }


        [HttpGet("{id}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult<SupervisorDetailsDTO>> Get(string id)
        {
            var Supervisor = await _repository.ReadAsync(id);

            if (Supervisor == null) return NotFound();

            else return Supervisor;
        }

        [HttpPost]
        public async Task<string> Post([FromBody] SupervisorCreateDTO Supervisor)
        {
            return await _repository.CreateAsync(Supervisor);
        }

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