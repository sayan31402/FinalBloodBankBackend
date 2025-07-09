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


    [TestClass]

    public class PersonControllerTests
    {

        private Mock<IPersonRepository> _mockRepo;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<PersonController>> _mockLogger;
        private PersonController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IPersonRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<PersonController>>();
            _controller = new PersonController(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task GetAllPersons_ReturnsOk_WithPersonDTOList()
        {
            var persons = new List<Person> { new Person { PersonId = 1 }, new Person { PersonId = 2 } };
            var personDTOs = new List<PersonDTO> { new PersonDTO { PersonId = 1 }, new PersonDTO { PersonId = 2 } };

            _mockRepo.Setup(r => r.GetAllPersons()).ReturnsAsync(persons);
            _mockMapper.Setup(m => m.Map<List<PersonDTO>>(persons)).Returns(personDTOs);

            var result = await _controller.GetAllPersons();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            CollectionAssert.AreEqual(personDTOs, (List<PersonDTO>)okResult.Value);
        }

        [TestMethod]
        public async Task GetPersonById_ReturnsOk_WhenPersonExists()
        {
            var person = new Person { PersonId = 1 };
            var personDTO = new PersonDTO { PersonId = 1 };

            _mockRepo.Setup(r => r.GetPersonById(1)).ReturnsAsync(person);
            _mockMapper.Setup(m => m.Map<PersonDTO>(person)).Returns(personDTO);

            var result = await _controller.GetPersonById(1);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(personDTO, okResult.Value);
        }

        [TestMethod]
        public async Task GetPersonById_ReturnsNotFound_WhenPersonDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetPersonById(1)).ReturnsAsync((Person)null);

            var result = await _controller.GetPersonById(1);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task GetPersonByName_ReturnsOk_WhenFound()
        {
            var persons = new List<Person> { new Person { Name = "John" } };
            var personDTOs = new List<PersonDTO> { new PersonDTO { Name = "John" } };

            _mockRepo.Setup(r => r.GetPersonByName("John")).ReturnsAsync(persons);
            _mockMapper.Setup(m => m.Map<List<PersonDTO>>(persons)).Returns(personDTOs);

            var result = await _controller.GetPersonByName("John");

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            CollectionAssert.AreEqual(personDTOs, (List<PersonDTO>)okResult.Value);
        }

        [TestMethod]
        public async Task AddPerson_ReturnsOk_WhenSuccessful()
        {
            var personDTO = new PersonDTO { PersonId = 1 };
            var person = new Person { PersonId = 1 };

            _mockMapper.Setup(m => m.Map<Person>(personDTO)).Returns(person);
            _mockRepo.Setup(r => r.AddPerson(person)).ReturnsAsync(true);

            var result = await _controller.AddPerson(personDTO);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
        }
    }
}




    

