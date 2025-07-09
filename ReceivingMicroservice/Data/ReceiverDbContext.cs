using ReceiverMicroservice.Models;
using Microsoft.EntityFrameworkCore;

namespace ReceiverMicroservice.Data
{
    public class ReceiverDbContext : DbContext
    {
        public ReceiverDbContext(DbContextOptions<ReceiverDbContext> options) : base(options) { }

        //--------------------------------------------------------------------------
        public DbSet<Receiver> Receivers { get; set; }
    }

}
