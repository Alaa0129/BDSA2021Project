using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorApp.Api.Controllers;
using BlazorApp.Core;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BlazorApp.Api.Tests.Controllers
{
    public class ProjectControllerTests
    {
        [Fact]
        public async Task Get_returns_projects()
        {
            //Given
            var logger = new Mock<ILogger<ProjectController>>();
            var repository = new Mock<IProjectRepository>();

            var expected = Array.Empty<ProjectDetailsDTO>();
            repository.Setup(m => m.ReadAsync()).ReturnsAsync(expected);
            var controller = new ProjectController(logger.Object, repository.Object);

            //When
            var actual = await controller.Get();

            //Then
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Get_given_valid_id_returns_project()
        {
            //Given
            var logger = new Mock<ILogger<ProjectController>>();
            var repository = new Mock<IProjectRepository>();

            var expected = new ProjectDetailsDTO(1, "Project One", "Project One Description", new SupervisorDTO("SupervisorId", "Supervisor Name"), new[] {new StudentDTO("StudentId", "Student Name")}, new[] {"Tag1"});
            repository.Setup(m => m.ReadAsync(1)).ReturnsAsync(expected);
            var controller = new ProjectController(logger.Object, repository.Object);
            //When
            var actual = await controller.Get(1);
            //Then
            Assert.Equal(expected, actual.Value);
        }
    }
}