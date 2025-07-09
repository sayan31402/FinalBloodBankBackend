using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserMicroservice.Controllers;
using UserMicroservice.Models.DTO;
using UserMicroservice.Services.DAO;

namespace UnitTest
{
    [TestClass]
    public class AuthControllerTests
    {


        private Mock<IAuthRepo> _mockAuthRepo;
        private AuthController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockAuthRepo = new Mock<IAuthRepo>();
            _controller = new AuthController(_mockAuthRepo.Object);
        }

        [TestMethod]
        public void Login_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var loginModel = new LoginModel { UserName = "", Password = "" };

            // Act
            var result = _controller.Login(loginModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginModel = new LoginModel { UserName = "test", Password = "wrong" };
            _mockAuthRepo.Setup(repo => repo.Login(loginModel)).Returns((AuthResponseModel)null);

            // Act
            var result = _controller.Login(loginModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
        }

        [TestMethod]
        public void Login_ValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var loginModel = new LoginModel { UserName = "test", Password = "pass" };
            var authResponse = new AuthResponseModel
            {
                Id = 1,
                UserName = "test",
                Token = "fake-jwt-token",
                Role = "ADMIN"
            };

            _mockAuthRepo.Setup(repo => repo.Login(loginModel)).Returns(authResponse);

            // Act
            var result = _controller.Login(loginModel) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            var response = result.Value as AuthResponseModel;
            Assert.AreEqual("test", response.UserName);
            Assert.AreEqual("fake-jwt-token", response.Token);
        }
    }
}



    

