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
    public class StudentControllerTests
    {
        [Fact]
        public async Task Get_returns_students()
        {
            //Given
            var logger = new Mock<ILogger<StudentController>>();
            var repository = new Mock<IStudentRepository>();

            var expected = Array.Empty<StudentDTO>();
            repository.Setup(m => m.ReadAsync()).ReturnsAsync(expected);
            var controller = new StudentController(logger.Object, repository.Object);
            //When
            var actual = await controller.Get();

            //Then
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Get_giving_non_valid_id_returns_NotFound()
        {
            //Given
            var logger = new Mock<ILogger<StudentController>>();
            var repository = new Mock<IStudentRepository>();

            repository.Setup(m => m.ReadAsync("13123")).ReturnsAsync(default(StudentDetailsDTO));
            var controller = new StudentController(logger.Object, repository.Object);
            //When
            var actual = await controller.Get("13123");

            //Then
            Assert.IsType<NotFoundResult>(actual.Result);
        }

        [Fact]
        public async Task Get_giving_valid_id_returns_student()
        {
            //Given
            var logger = new Mock<ILogger<StudentController>>();
            var repository = new Mock<IStudentRepository>();

            var student = new StudentDetailsDTO("StudentId", "Student Name", new ProjectDTO(1, "Title", "Desc", "SupervisorId"), new[] { new RequestDTO(1, "Title", "Desc", "StudentId", "SupervisorId") });
            repository.Setup(m => m.ReadAsync("StudentId")).ReturnsAsync(student);
            var controller = new StudentController(logger.Object, repository.Object);
            //When
            var actual = await controller.Get("StudentId");

            //Then
            Assert.Equal(student, actual.Value);
        }


        [Fact]
        public async Task Put_updates_student()
        {
            //Given
            var logger = new Mock<ILogger<StudentController>>();
            var repository = new Mock<IStudentRepository>();

            var student = new StudentUpdateDTO();
            repository.Setup(m => m.UpdateAsync(student)).ReturnsAsync(HttpStatusCode.OK);
            var controller = new StudentController(logger.Object, repository.Object);

            //When
            var result = await controller.Put(student);

            //Then
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Put_given_unkown_student_returns_returns_NotFound()
        {
            //Given
            var logger = new Mock<ILogger<StudentController>>();
            var repository = new Mock<IStudentRepository>();

            var student = new StudentUpdateDTO();
            repository.Setup(m => m.UpdateAsync(student)).ReturnsAsync(HttpStatusCode.NotFound);
            var controller = new StudentController(logger.Object, repository.Object);

            //When
            var result = await controller.Put(student);

            //Then
            Assert.IsType<NotFoundResult>(result);
        }
    }
}