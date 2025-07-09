using Microsoft.EntityFrameworkCore;
using DonationMicroservice.Models;

namespace DonationMicroservice.Data
{
    public class DonationDbContext : DbContext
    {
        public DonationDbContext(DbContextOptions<DonationDbContext> options) : base(options) { }

        //--------------------------------------------------------------------------
        public DbSet<Donor> Donors { get; set; }
    }

}
