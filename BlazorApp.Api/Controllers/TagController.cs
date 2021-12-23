using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorApp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlazorApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly ILogger<TagController> _logger;
        private readonly ITagRepository _repository;

        public TagController(ILogger<TagController> logger, ITagRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }
        /* Returns all tags in the repository through a GET request */
        [HttpGet("all")]
        public async Task<IEnumerable<TagDetailsDTO>> Get()
        {
            return await _repository.ReadAsyncAll();
        }

    }
}