using System;
using System.Net;
using System.Threading.Tasks;
using BlazorApp.Api.Controllers;
using BlazorApp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BlazorApp.Api.Tests.Controllers
{
    public class SupervisorControllerTests
    {
        [Fact]
        public async Task Get_returns_Supervisors_from_repo()
        {
            //Given
            var logger = new Mock<ILogger<SupervisorController>>();
            var repository = new Mock<ISupervisorRepository>();

            var expected = Array.Empty<SupervisorDTO>();
            repository.Setup(m => m.ReadAsync()).ReturnsAsync(expected);
            var controller = new SupervisorController(logger.Object, repository.Object);
            //When
            var actual = await controller.Get();

            //Then
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Get_giving_non_valid_id_returns_NotFound()
        {
            //Given
            var logger = new Mock<ILogger<SupervisorController>>();
            var repository = new Mock<ISupervisorRepository>();

            repository.Setup(m => m.ReadAsync("13123")).ReturnsAsync(default(SupervisorDetailsDTO));
            var controller = new SupervisorController(logger.Object, repository.Object);
            //When
            var actual = await controller.Get("13123");

            //Then
            Assert.IsType<NotFoundResult>(actual.Result);
        }

        [Fact]
        public async Task Get_giving_valid_id_returns_supervisor()
        {
            //Given
            var logger = new Mock<ILogger<SupervisorController>>();
            var repository = new Mock<ISupervisorRepository>();

            var supervisor = new SupervisorDetailsDTO("SupervisorId", "John", new[] {new ProjectDTO(1, "Title", "Desc", "SupervisorId")}, new[] {new RequestDTO(1, "Title", "Desc", "StudentId", "SupervisorId")} );
            repository.Setup(m => m.ReadAsync("SupervisorId")).ReturnsAsync(supervisor);
            var controller = new SupervisorController(logger.Object, repository.Object);
            //When
            var actual = await controller.Get("SupervisorId");

            //Then
            Assert.Equal(supervisor ,actual.Value);
        }


        [Fact]
        public async Task Put_updates_Supervisor()
        {
            //Given
            var logger = new Mock<ILogger<SupervisorController>>();
            var repository = new Mock<ISupervisorRepository>();

            var supervisor = new SupervisorUpdateDTO();
            repository.Setup(m => m.UpdateAsync(supervisor)).ReturnsAsync(HttpStatusCode.OK);
            var controller = new SupervisorController(logger.Object, repository.Object);

            //When
            var result = await controller.Put(supervisor);

            //Then
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Put_given_unkown_Supervisor_returns_returns_NotFound()
        {
            //Given
            var logger = new Mock<ILogger<SupervisorController>>();
            var repository = new Mock<ISupervisorRepository>();

            var supervisor = new SupervisorUpdateDTO();
            repository.Setup(m => m.UpdateAsync(supervisor)).ReturnsAsync(HttpStatusCode.NotFound);
            var controller = new SupervisorController(logger.Object ,repository.Object);

            //When
            var result = await controller.Put(supervisor);

            //Then
            Assert.IsType<NotFoundResult>(result);
        }
    }
}