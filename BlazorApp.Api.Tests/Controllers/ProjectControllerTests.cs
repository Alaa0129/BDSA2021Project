using System.Threading.Tasks;
using BlazorApp.Api.Controllers;
using BlazorApp.Core;
using Moq;
using Xunit;

namespace BlazorApp.Api.Tests.Controllers
{
    public class ProjectControllerTests
    {
        [Fact]
        public async Task Get_returns_projects_from_repository()
        {
            //Given
            var expected = new ProjectDetailsDTO[0];

            var repository = new Mock<IProjectRepository>();
            repository.Setup(m => m.ReadAsyncAll()).ReturnsAsync(expected);
            var controller = new ProjectController(repository.Object);

            //When
            var actual = await controller.Get();

            //Then
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Get_given_valid_id_returns_project_from_repository()
        {
            //Given

            var tags = new[] { "tag1", "tag2", "tag3" };
            var expected = new ProjectDetailsDTO(1, "Project One", "Project One Description", 1, 4,tags);

            var repository = new Mock<IProjectRepository>();
            repository.Setup(m => m.ReadAsync(1)).ReturnsAsync(expected);
            var controller = new ProjectController(repository.Object);
            
            //When
            var actual = await controller.Get(1);
            
            //Then
            Assert.Equal(expected, actual.Value);
        }

        [Fact]
        
        public async Task Get_given_tag_name_returns_projects()
        {

            var expected = new ProjectDetailsDTO[0];

            var repository = new Mock<IProjectRepository>();
            repository.Setup(m => m.ReadAsyncAllByTagName("tag1")).ReturnsAsync(expected);
            var controller = new ProjectController(repository.Object);

            var actual = await controller.Get("tag1");

            Assert.Equal(expected, actual);


        }
    }
}