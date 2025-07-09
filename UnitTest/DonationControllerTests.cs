using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PersonMicroservice.Controllers;
using PersonMicroservice.Models;
using PersonMicroservice.Models.DTO;
using PersonMicroservice.Repository;

namespace UnitTest


{

    [TestClass]
    public class DonationControllerTests
    {



        private Mock<IDonationRepository> _mockRepo;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<DonationController>> _mockLogger;
        private DonationController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IDonationRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<DonationController>>();
            _controller = new DonationController(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task GetAllDonors_ReturnsOk_WithDonorDTOList()
        {
            var donors = new List<Donor> { new Donor { DonationId = 1 }, new Donor { DonationId = 2 } };
            var donorDTOs = new List<DonorDTO> { new DonorDTO { DonationId = 1 }, new DonorDTO { DonationId = 2 } };

            _mockRepo.Setup(r => r.GetAllDonors()).ReturnsAsync(donors);
            _mockMapper.Setup(m => m.Map<List<DonorDTO>>(donors)).Returns(donorDTOs);

            var result = await _controller.GetAllDonors();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            CollectionAssert.AreEqual(donorDTOs, (List<DonorDTO>)okResult.Value);
        }

        [TestMethod]
        public async Task GetDonorById_ReturnsOk_WhenDonorExists()
        {
            var donor = new Donor { DonationId = 1 };
            var donorDTO = new DonorDTO { DonationId = 1 };

            _mockRepo.Setup(r => r.GetDonorById(1)).ReturnsAsync(donor);
            _mockMapper.Setup(m => m.Map<DonorDTO>(donor)).Returns(donorDTO);

            var result = await _controller.GetDonorById(1);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(donorDTO, okResult.Value);
        }

        [TestMethod]
        public async Task AddDonor_ReturnsOk_WhenSuccessful()
        {
            var donorDTO = new DonorDTO { DonationId = 1 };
            var donor = new Donor { DonationId = 1 };

            _mockMapper.Setup(m => m.Map<Donor>(donorDTO)).Returns(donor);
            _mockRepo.Setup(r => r.AddDonor(donor)).ReturnsAsync(true);

            var result = await _controller.AddDonor(donorDTO);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task UpdateDonor_ReturnsOk_WhenSuccessful()
        {
            var donorDTO = new DonorDTO { DonationId = 1 };
            var donor = new Donor { DonationId = 1 };

            _mockRepo.Setup(r => r.GetDonorById(1)).ReturnsAsync(new Donor { DonationId = 1 });
            _mockMapper.Setup(m => m.Map<Donor>(donorDTO)).Returns(donor);
            _mockRepo.Setup(r => r.UpdateDonor(donor, 1)).ReturnsAsync(true);

            var result = await _controller.UpdateDonor(1, donorDTO);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task DeleteDonor_ReturnsOk_WhenSuccessful()
        {
            var donor = new Donor { DonationId = 1, Person = new Person { BloodGroup = "A+" }, Quantity = 1 };

            _mockRepo.Setup(r => r.GetDonorById(1)).ReturnsAsync(donor);
            _mockRepo.Setup(r => r.DeleteDonor(1)).ReturnsAsync(true);

            var result = await _controller.DeleteDonor(1);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}






