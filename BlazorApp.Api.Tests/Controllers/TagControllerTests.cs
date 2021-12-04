using System;
using System.Threading.Tasks;
using BlazorApp.Api.Controllers;
using BlazorApp.Core;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BlazorApp.Api.Tests.Controllers
{
    public class TagControllerTests
    {
        [Fact]
        public async Task Get_returns_tags_from_repository()
        {
            //Given
            var logger = new Mock<ILogger<TagController>>();
            var repository = new Mock<ITagRepository>();

            var expected = Array.Empty<TagDetailsDTO>();
            repository.Setup(t => t.ReadAsyncAll()).ReturnsAsync(expected);
            var controller = new TagController(logger.Object, repository.Object);

            //When
            var actual = await controller.Get();

            //Then
            Assert.Equal(expected, actual);

        }

    }
}