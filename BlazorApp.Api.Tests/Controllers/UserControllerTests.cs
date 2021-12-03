using System.Net;
using System.Threading.Tasks;
using BlazorApp.Api.Controllers;
using BlazorApp.Core;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BlazorApp.Api.Tests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public async Task Get_returns_user_from_repo()
        {
            //Given
            var expected = new UserDTO[0];

            var repository = new Mock<IUserRepository>();
            repository.Setup(m => m.ReadAsync()).ReturnsAsync(expected);
            var controller = new UserController(repository.Object);
            //When
            var actual = await controller.Get();

            //Then
            Assert.Equal(expected, actual);
        }


        [Fact]
        public async Task Put_updates_user()
        {
            //Given
            var user = new UserUpdateDTO();
            var repository = new Mock<IUserRepository>();
            repository.Setup(m => m.UpdateAsync(user)).ReturnsAsync(HttpStatusCode.OK);
            var controller = new UserController(repository.Object);

            //When
            var result = await controller.Put(1, user);

            //Then
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Put_given_unkown_id_returns_returns_NotFound()
        {
            //Given
            var user = new UserUpdateDTO();
            var repository = new Mock<IUserRepository>();
            repository.Setup(m => m.UpdateAsync(user)).ReturnsAsync(HttpStatusCode.NotFound);
            var controller = new UserController(repository.Object);

            //When
            var result = await controller.Put(1, user);

            //Then
            Assert.IsType<NotFoundResult>(result);
        }
    }
}