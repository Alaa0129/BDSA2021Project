using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BlazorApp.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace BlazorApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {

        private readonly ILogger<StudentController> _logger;
        private readonly IStudentRepository _repository;

        public StudentController(ILogger<StudentController> logger, IStudentRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        /* Returns all students in the repository through a GET request */
        [HttpGet("all")]
        public async Task<IEnumerable<StudentDTO>> Get()
        {
            return await _repository.ReadAsync();
        }

        /* Given a studentID, returns the student if found in the repository through a GET request */
        [HttpGet("{id}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult<StudentDetailsDTO>> Get(string id)
        {
            var student = await _repository.ReadAsync(id);

            if (student == null) return NotFound();

            else return student;
        }

        /* Create a student in the repo through a POST request*/
        [HttpPost]
        public async Task<string> Post([FromBody] StudentCreateDTO student)
        {
            return await _repository.CreateAsync(student);
        }

        /* updates students through a PUT request */
        [HttpPut("update")]
        public async Task<ActionResult> Put([FromBody] StudentUpdateDTO student)
        {
            var response = await _repository.UpdateAsync(student);

            if (response == HttpStatusCode.OK)
            {
                return NoContent();
            }

            return NotFound();
        }

        /* Assigns a project to a student with given studentID through PUT request */
        [HttpPut("updateProject/{projectId}")]
        public async Task<ActionResult> Put([FromBody] string studentId, int projectId)
        {
            var response = await _repository.UpdateProjectAsync(studentId, projectId);

            if (response == HttpStatusCode.OK)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}