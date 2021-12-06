using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BlazorApp.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;

namespace BlazorApp.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    // [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class ProjectController : ControllerBase
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly IProjectRepository _repository;

        public ProjectController(ILogger<ProjectController> logger, IProjectRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        // [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IEnumerable<ProjectDetailsDTO>> Get()
        {
            return await _repository.ReadAsync();
        }

        // [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDetailsDTO>> Get(int id)
        {
            var project = await _repository.ReadAsync(id);

            if (project == null) return NotFound();

            return project;
        }

        [HttpPost]
        public async Task<int> Post([FromBody] ProjectCreateDTO project)
        {
            return await _repository.CreateAsync(project);
        }

        // [Authorize(Roles = "Student")]
        [HttpPut("update")]
        public async Task<ActionResult> Put([FromBody] ProjectUpdateDTO project)
        {
            var response = await _repository.UpdateAsync(project);

            if (response == HttpStatusCode.OK)
            {
                return Ok();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _repository.DeleteAsync(id);

            if (response == HttpStatusCode.OK)
            {
                return Ok();
            }

            return NotFound();
        }

    }
}