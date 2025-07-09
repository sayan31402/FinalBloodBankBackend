//using NUnit.Framework;
//using Microsoft.EntityFrameworkCore;

//using System.Threading.Tasks;
//using System.Linq;
//using Microsoft.AspNetCore.Routing;

//using DonationMicroservice.Data;
//using DonationMicroservice.Models;

//namespace DonationMicroservice.Repository
//{
//    [TestFixture]
//    public class DonationRepositoryTest
//    {
//        private DonationDbContext _context;
//        private DonationRepository _repository;

//        [SetUp]
//        public void SetUp()
//        {
//            var options = new DbContextOptionsBuilder<DonationDbContext>()
//                .UseInMemoryDatabase(databaseName: "DonationDb_Test")
//                .Options;
//            _context = new DonationDbContext(options);
//            _repository = new DonationRepository(_context);

//            // Seed data
//            _context.Donors.AddRange(
//                new Donor { DonationId = 1, PersonId = 100, Quantity = 250, RBCCount = 4.5, WBCCount = 6, PlateletCount = 250, DonationDateTime = System.DateTime.Now },
//                new Donor { DonationId = 2, PersonId = 200, Quantity = 300, RBCCount = 5.0, WBCCount = 7, PlateletCount = 300, DonationDateTime = System.DateTime.Now }
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
//        public async Task GetAllDonors_ReturnsAllDonors()
//        {
//            var donors = await _repository.GetAllDonors();
//            Assert.AreEqual(2, donors.Count());
//        }

//        [Test]
//        public async Task GetDonorById_ExistingId_ReturnsDonor()
//        {
//            var donor = await _repository.GetDonorById(1);
//            Assert.IsNotNull(donor);
//            Assert.AreEqual(100, donor.PersonId);
//        }

//        [Test]
//        public async Task GetDonorById_NonExistingId_ReturnsNull()
//        {
//            var donor = await _repository.GetDonorById(999);
//            Assert.IsNull(donor);
//        }

//        [Test]
//        public async Task AddDonor_AddsDonorSuccessfully()
//        {
//            var newDonor = new Donor
//            {
//                DonationId = 3,
//                PersonId = 300,
//                Quantity = 350,
//                RBCCount = 5.2,
//                WBCCount = 7.5,
//                PlateletCount = 320,
//                DonationDateTime = System.DateTime.Now
//            };

//            var result = await _repository.AddDonor(newDonor);

//            Assert.IsTrue(result);
//            Assert.AreEqual(3, _context.Donors.Count());
//            Assert.IsNotNull(_context.Donors.FirstOrDefault(d => d.DonationId == 3));
//        }

//        [Test]
//        public async Task UpdateDonor_ExistingId_UpdatesSuccessfully()
//        {
//            var updateDonor = new Donor
//            {
//                PersonId = 101,
//                Quantity = 255,
//                RBCCount = 4.8,
//                WBCCount = 6.2,
//                PlateletCount = 255,
//                DonationDateTime = System.DateTime.Now
//            };

//            var result = await _repository.UpdateDonor(updateDonor, 1);

//            Assert.IsTrue(result);
//            var donor = _context.Donors.FirstOrDefault(d => d.DonationId == 1);
//            Assert.AreEqual(101, donor.PersonId);
//            Assert.AreEqual(255, donor.Quantity);
//        }

//        [Test]
//        public async Task UpdateDonor_NonExistingId_ReturnsFalse()
//        {
//            var updateDonor = new Donor
//            {
//                PersonId = 999,
//                Quantity = 123,
//                RBCCount = 4.0,
//                WBCCount = 5.0,
//                PlateletCount = 100,
//                DonationDateTime = System.DateTime.Now
//            };

//            var result = await _repository.UpdateDonor(updateDonor, 999);

//            Assert.IsFalse(result);
//        }

//        [Test]
//        public async Task DeleteDonor_ExistingId_DeletesSuccessfully()
//        {
//            var result = await _repository.DeleteDonor(1);

//            Assert.IsTrue(result);
//            Assert.AreEqual(1, _context.Donors.Count());
//            Assert.IsNull(_context.Donors.FirstOrDefault(d => d.DonationId == 1));
//        }

//        [Test]
//        public async Task DeleteDonor_NonExistingId_ReturnsFalse()
//        {
//            var result = await _repository.DeleteDonor(999);

//            Assert.IsFalse(result);
//        }
//    }
//}
