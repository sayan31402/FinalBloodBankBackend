//using NUnit.Framework;
//using Microsoft.EntityFrameworkCore;

//using System.Threading.Tasks;
//using System.Linq;
//using System;
//using Microsoft.AspNetCore.Routing;

//using ReceiverMicroservice.Data;
//using ReceiverMicroservice.Models;

//namespace ReceiverMicroservice.Repository
//{
//    [TestFixture]
//    public class ReceiverRepositoryTest
//    {
//        private ReceiverDbContext _context;
//        private ReceiverRepository _repository;

//        [SetUp]
//        public void SetUp()
//        {
//            var options = new DbContextOptionsBuilder<ReceiverDbContext>()
//                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                .Options;
//            _context = new ReceiverDbContext(options);
//            _repository = new ReceiverRepository(_context);

//            // Seed data
//            _context.Receivers.AddRange(
//                new Receiver { ReceiverId = 1, PersonId = 111, ReceiverDateTime = DateTime.UtcNow, Quantity = 500, HospitalName = "City Hospital" },
//                new Receiver { ReceiverId = 2, PersonId = 222, ReceiverDateTime = DateTime.UtcNow, Quantity = 300, HospitalName = "County Hospital" }
//            );
//            _context.SaveChanges();
//        }

//        [TearDown]
//        public void TearDown()
//        {
//            _context.Database.EnsureDeleted();
//            _context.Dispose();
//        }

//        [Test]
//        public async Task GetAllReceivers_ReturnsAllReceivers()
//        {
//            var receivers = await _repository.GetAllReceivers();
//            Assert.AreEqual(2, receivers.Count());
//        }

//        [Test]
//        public async Task GetReceiverById_ExistingId_ReturnsReceiver()
//        {
//            var receiver = await _repository.GetReceiverById(1);
//            Assert.IsNotNull(receiver);
//            Assert.AreEqual(111, receiver.PersonId);
//        }

//        [Test]
//        public async Task GetReceiverById_NonExistingId_ReturnsNull()
//        {
//            var receiver = await _repository.GetReceiverById(999);
//            Assert.IsNull(receiver);
//        }

//        [Test]
//        public async Task AddReceiver_AddsReceiverSuccessfully()
//        {
//            var newReceiver = new Receiver
//            {
//                ReceiverId = 3,
//                PersonId = 333,
//                ReceiverDateTime = DateTime.UtcNow,
//                Quantity = 700,
//                HospitalName = "General Hospital"
//            };

//            var result = await _repository.AddReceiver(newReceiver);

//            Assert.IsTrue(result);
//            Assert.AreEqual(3, _context.Receivers.Count());
//            Assert.IsNotNull(_context.Receivers.FirstOrDefault(r => r.ReceiverId == 3));
//        }

//        [Test]
//        public async Task UpdateReceiver_ExistingId_UpdatesSuccessfully()
//        {
//            var updateReceiver = new Receiver
//            {
//                PersonId = 444,
//                ReceiverDateTime = DateTime.UtcNow.AddHours(1),
//                Quantity = 600,
//                HospitalName = "Updated Hospital"
//            };

//            var result = await _repository.UpdateReceiver(updateReceiver, 1);

//            Assert.IsTrue(result);
//            var receiver = _context.Receivers.FirstOrDefault(r => r.ReceiverId == 1);
//            Assert.AreEqual(444, receiver.PersonId);
//            Assert.AreEqual("Updated Hospital", receiver.HospitalName);
//            Assert.AreEqual(600, receiver.Quantity);
//        }

//        [Test]
//        public async Task UpdateReceiver_NonExistingId_ReturnsFalse()
//        {
//            var updateReceiver = new Receiver
//            {
//                PersonId = 555,
//                ReceiverDateTime = DateTime.UtcNow,
//                Quantity = 100,
//                HospitalName = "NonExistent Hospital"
//            };

//            var result = await _repository.UpdateReceiver(updateReceiver, 999);

//            Assert.IsFalse(result);
//        }

//        [Test]
//        public async Task DeleteReceiver_ExistingId_DeletesSuccessfully()
//        {
//            var result = await _repository.DeleteReceiver(2);

//            Assert.IsTrue(result);
//            Assert.AreEqual(1, _context.Receivers.Count());
//            Assert.IsNull(_context.Receivers.FirstOrDefault(r => r.ReceiverId == 2));
//        }

//        [Test]
//        public async Task DeleteReceiver_NonExistingId_ReturnsFalse()
//        {
//            var result = await _repository.DeleteReceiver(999);

//            Assert.IsFalse(result);
//        }
//    }
//}
