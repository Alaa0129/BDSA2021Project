using BlazorApp.Api.Controllers;
using BlazorApp.Core;
using Moq;
using Xunit;

namespace BlazorApp.Api.Tests.Controllers
{
    public class RequestControllerTests
    {
        [Fact]
        public async void Given_a_list_of_requests_in_the_repo_returns_the_list_with_requests()
        {
        //Given
            var expected = new RequestDetailsDTO[0];

            var repo = new Mock<IRequestRepository>();
            repo.Setup(s => s.ReadAsync()).ReturnsAsync(expected);
            var controller = new RequestController(repo.Object);

        //When
            var actual = await controller.Get();

        //Then
            Assert.Equal(expected, actual);
        }
    

        [Fact]
        public async void Given_a_request_id_returns_the_request_from_repo()
        {
        //Given
            var expected = new RequestDetailsDTO(1, "request title", "request description", 9);

            var repo = new Mock<IRequestRepository>();
            repo.Setup(s => s.ReadAsync(1)).ReturnsAsync(expected);
            var controller = new RequestController(repo.Object);
        
        //When
            var actual = await controller.Get(1);

        //Then
            Assert.Equal(expected, actual.Value);
        }
    }
}