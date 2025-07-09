using PersonMicroservice.Models;
using Microsoft.EntityFrameworkCore;

namespace PersonMicroservice.Data;

public class PersonDbContext : DbContext
{
    public PersonDbContext(DbContextOptions<PersonDbContext> options) : base(options) { }

    //--------------------------------------------------------------------------
    public DbSet<Person> Persons { get; set; }
    public DbSet<Donor> Donors { get; set; }
    public DbSet<Receiver> Receivers { get; set; }
    public DbSet<Stock> Stocks { get; set; }
}
