using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonMicroservice.Controllers;
using PersonMicroservice.Models.DTO;
using PersonMicroservice.Models;
using PersonMicroservice.Repository;
using Microsoft.Extensions.Logging;

namespace UnitTest
{
    public class ReceiverControllerTests
    {


        private Mock<IReceiverRepository> _mockRepo;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<ReceiverController>> _mockLogger;
        private ReceiverController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IReceiverRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<ReceiverController>>();
            _controller = new ReceiverController(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task GetAllReceivers_ReturnsOk_WithReceiverDTOList()
        {
            var receivers = new List<Receiver> { new Receiver { ReceiverId = 1 }, new Receiver { ReceiverId = 2 } };
            var receiverDTOs = new List<ReceiverDTO> { new ReceiverDTO { ReceiverId = 1 }, new ReceiverDTO { ReceiverId = 2 } };

            _mockRepo.Setup(r => r.GetAllReceivers()).ReturnsAsync(receivers);
            _mockMapper.Setup(m => m.Map<List<ReceiverDTO>>(receivers)).Returns(receiverDTOs);

            var result = await _controller.GetAllReceivers();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            CollectionAssert.AreEqual(receiverDTOs, (List<ReceiverDTO>)okResult.Value);
        }

        [TestMethod]
        public async Task GetReceiverById_ReturnsOk_WhenReceiverExists()
        {
            var receiver = new Receiver { ReceiverId = 1 };
            var receiverDTO = new ReceiverDTO { ReceiverId = 1 };

            _mockRepo.Setup(r => r.GetReceiverById(1)).ReturnsAsync(receiver);
            _mockMapper.Setup(m => m.Map<ReceiverDTO>(receiver)).Returns(receiverDTO);

            var result = await _controller.GetReceiverById(1);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(receiverDTO, okResult.Value);
        }

        [TestMethod]
        public async Task AddReceiver_ReturnsOk_WhenSuccessful()
        {
            var receiverDTO = new ReceiverDTO { ReceiverId = 1 };
            var receiver = new Receiver { ReceiverId = 1 };

            _mockMapper.Setup(m => m.Map<Receiver>(receiverDTO)).Returns(receiver);
            _mockRepo.Setup(r => r.AddReceiver(receiver)).ReturnsAsync(true);

            var result = await _controller.AddReceiver(receiverDTO);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task UpdateReceiver_ReturnsOk_WhenSuccessful()
        {
            var receiverDTO = new ReceiverDTO { ReceiverId = 1 };
            var receiver = new Receiver { ReceiverId = 1 };

            _mockRepo.Setup(r => r.GetReceiverById(1)).ReturnsAsync(new Receiver { ReceiverId = 1 });
            _mockMapper.Setup(m => m.Map<Receiver>(receiverDTO)).Returns(receiver);
            _mockRepo.Setup(r => r.UpdateReceiver(receiver, 1)).ReturnsAsync(true);

            var result = await _controller.UpdateReceiver(1, receiverDTO);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task DeleteReceiver_ReturnsOk_WhenSuccessful()
        {
            var receiver = new Receiver { ReceiverId = 1, Person = new Person { BloodGroup = "A+" }, Quantity = 1 };

            _mockRepo.Setup(r => r.GetReceiverById(1)).ReturnsAsync(receiver);
            _mockRepo.Setup(r => r.DeleteReceiver(1)).ReturnsAsync(true);

            var result = await _controller.DeleteReceiver(1);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}



    

