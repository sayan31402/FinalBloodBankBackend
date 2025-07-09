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

namespace UnitTest
{
    [TestClass]
    public class StockControllerTests
    {

        private Mock<IStockRepo> _mockRepo;
        private Mock<IMapper> _mockMapper;
        private StockController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IStockRepo>();
            _mockMapper = new Mock<IMapper>();
            _controller = new StockController(_mockRepo.Object, _mockMapper.Object);
        }

        [TestMethod]
        public async Task GetAllStocks_ReturnsOk_WithStockDTOList()
        {
            var stocks = new List<Stock> { new Stock { BloodGroup = "A+" }, new Stock { BloodGroup = "B+" } };
            var stockDTOs = new List<StockGetDTO> { new StockGetDTO { BloodGroup = "A+" }, new StockGetDTO { BloodGroup = "B+" } };

            _mockRepo.Setup(r => r.GetAllStocks()).ReturnsAsync(stocks);
            _mockMapper.Setup(m => m.Map<IEnumerable<StockGetDTO>>(stocks)).Returns(stockDTOs);

            var result = await _controller.GetAllStocks();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            CollectionAssert.AreEqual((List<StockGetDTO>)stockDTOs, (List<StockGetDTO>)okResult.Value);
        }

        [TestMethod]
        public async Task GetStockByBloodGroup_ReturnsOk_WhenStockExists()
        {
            var stock = new Stock { BloodGroup = "A+" };
            var stockDTO = new StockGetDTO { BloodGroup = "A+" };

            _mockRepo.Setup(r => r.GetStockByBloodGroup("A+")).ReturnsAsync(stock);
            _mockMapper.Setup(m => m.Map<StockGetDTO>(stock)).Returns(stockDTO);

            var result = await _controller.GetStockByBloodGroup("A+");

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(stockDTO, okResult.Value);
        }

        [TestMethod]
        public async Task CreateStock_ReturnsOk_WhenSuccessful()
        {
            var stockDTO = new StockCreateDTO { BloodGroup = "A+", Quantity = 10 };
            var stock = new Stock { BloodGroup = "A+", Quantity = 10 };

            _mockMapper.Setup(m => m.Map<Stock>(stockDTO)).Returns(stock);
            _mockRepo.Setup(r => r.CreateStock(stock)).ReturnsAsync(true);

            var result = await _controller.CreateStock(stockDTO);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task UpdateStock_ReturnsOk_WhenSuccessful()
        {
            var stockDTO = new StockUpdateDTO { Quantity = 5 };
            var stock = new Stock { Quantity = 5 };

            _mockMapper.Setup(m => m.Map<Stock>(stockDTO)).Returns(stock);
            _mockRepo.Setup(r => r.UpdateStock("A+", stock)).ReturnsAsync(true);

            var result = await _controller.UpdateStock("A+", stockDTO);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task DeleteStock_ReturnsOk_WhenSuccessful()
        {
            _mockRepo.Setup(r => r.DeleteStock("A+")).ReturnsAsync(true);

            var result = await _controller.DeleteStock("A+");

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}




    

