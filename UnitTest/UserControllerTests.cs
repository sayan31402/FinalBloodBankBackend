using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserMicroservice.Controllers;
using UserMicroservice.Models;
using UserMicroservice.Models.DTO;
using UserMicroservice.Services.DAO;

namespace UnitTest
{
    [TestClass]
    public class UserControllerTests
    {

        private Mock<IUserRepo> _mockUserRepo;
        private Mock<IMapper> _mockMapper;
        private UserController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockUserRepo = new Mock<IUserRepo>();
            _mockMapper = new Mock<IMapper>();
            _controller = new UserController(_mockUserRepo.Object, _mockMapper.Object);
        }

        [TestMethod]
        public async Task Registration_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("UserName", "Required");

            // Act
            var result = await _controller.Registration(new UserCreateDTO());

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Registration_ValidModel_ReturnsOk()
        {
            // Arrange
            var userDto = new UserCreateDTO { UserName = "test", Password = "pass" };
            var user = new User { UserName = "test", Password = "pass" };

            _mockMapper.Setup(m => m.Map<User>(userDto)).Returns(user);
            _mockUserRepo.Setup(r => r.CreateUser(user)).ReturnsAsync(true);

            // Act
            var result = await _controller.Registration(userDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetAllUsers_ReturnsOkWithUsers()
        {
            // Arrange
            var users = new List<User> { new User { UserId = 1, UserName = "test" } };
            _mockUserRepo.Setup(r => r.GetAllUsers()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(users, okResult.Value);
        }

        [TestMethod]
        public async Task GetUserById_UserExists_ReturnsOk()
        {
            // Arrange
            var user = new User { UserId = 1, UserName = "test" };
            var userDto = new UserGetDTO { UserId = 1, UserName = "test" };

            _mockUserRepo.Setup(r => r.GetUserById(1)).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<UserGetDTO>(user)).Returns(userDto);

            // Act
            var result = await _controller.GetUserById(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(userDto, okResult.Value);
        }

        [TestMethod]
        public async Task GetUserById_UserNotFound_ReturnsNotFound()
        {
            _mockUserRepo.Setup(r => r.GetUserById(1)).ReturnsAsync((User)null);

            var result = await _controller.GetUserById(1);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task DeleteUser_UserExists_ReturnsOk()
        {
            _mockUserRepo.Setup(r => r.DeleteUser(1)).ReturnsAsync(true);

            var result = await _controller.DeleteUser(1);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task DeleteUser_UserNotFound_ReturnsNotFound()
        {
            _mockUserRepo.Setup(r => r.DeleteUser(1)).ReturnsAsync(false);

            var result = await _controller.DeleteUser(1);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
    }
}



    

