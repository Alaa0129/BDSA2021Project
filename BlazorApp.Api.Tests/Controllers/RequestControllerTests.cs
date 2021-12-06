using System;
using BlazorApp.Api.Controllers;
using BlazorApp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BlazorApp.Api.Tests.Controllers
{
    public class RequestControllerTests
    {
        [Fact]
        public async void Get_returns_requests()
        {
            //Given
            var logger = new Mock<ILogger<RequestController>>();
            var repository = new Mock<IRequestRepository>();

            var expected = Array.Empty<RequestDTO>();
            repository.Setup(s => s.ReadAsync()).ReturnsAsync(expected);
            var controller = new RequestController(logger.Object, repository.Object);

            //When
            var actual = await controller.Get();

            //Then
            Assert.Equal(expected, actual);
        }


        [Fact]
        public async void Get_giving_non_valid_id_returns_NotFound()
        {
            //Given
            var logger = new Mock<ILogger<RequestController>>();
            var repository = new Mock<IRequestRepository>();

            repository.Setup(s => s.ReadAsync(33123)).ReturnsAsync(default(RequestDetailsDTO));
            var controller = new RequestController(logger.Object, repository.Object);

            //When
            var actual = await controller.Get(33123);

            //Then
            Assert.IsType<NotFoundResult>(actual.Result);
        }

        [Fact]
        public async void Get_giving_valid_id_returns_request()
        {
            //Given
            var logger = new Mock<ILogger<RequestController>>();
            var repository = new Mock<IRequestRepository>();

            var request = new RequestDetailsDTO(1, "Title", "Decs", new StudentDTO("StudentId", "Student Name"), new SupervisorDTO("SupervisorId", "Supervisor Name"));
            repository.Setup(s => s.ReadAsync(1)).ReturnsAsync(request);
            var controller = new RequestController(logger.Object, repository.Object);

            //When
            var actual = await controller.Get(1);

            //Then
            Assert.Equal(request, actual.Value);
        }


    }
}