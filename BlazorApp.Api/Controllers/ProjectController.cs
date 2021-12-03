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
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _repository;

        public ProjectController(IProjectRepository repository)
        {
            _repository = repository;
        }


        [HttpGet("all")]
        public async Task<IEnumerable<ProjectDetailsDTO>> Get()
        {
            return await _repository.ReadAsyncAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDetailsDTO>> Get(int id)
        {
            var project =  await _repository.ReadAsync(id);

            if (project == null) return NotFound();

            return project;
        }

        [HttpGet("{tagName}")]
        public async Task<IEnumerable<ProjectDetailsDTO>> Get(string tagName)
        {
            return await _repository.ReadAsyncAllByTagName(tagName);
   
        }


        [HttpPost]
        public async Task<int> Post([FromBody] ProjectCreateDTO project)
        {
            return await _repository.CreateAsync(project);
        }

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