using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorApp.Core;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagController : ControllerBase
    {
        private readonly ITagRepository _repository;

        public TagController(ITagRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("all")]
        public async Task<IEnumerable<TagDetailsDTO>> Get()
        {
            return await _repository.ReadAsyncAll();
        } 

    }
}