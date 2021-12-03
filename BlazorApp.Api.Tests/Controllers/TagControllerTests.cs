using System.Threading.Tasks;
using BlazorApp.Api.Controllers;
using BlazorApp.Core;
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

            var expected = new TagDetailsDTO[0];

            var repository = new Mock<ITagRepository>();

            repository.Setup(t => t.ReadAsyncAll()).ReturnsAsync(expected);

            var controller = new TagController(repository.Object);

            //When

            var actual = await controller.Get();

            //Then

            Assert.Equal(expected, actual);

        }

    }
}