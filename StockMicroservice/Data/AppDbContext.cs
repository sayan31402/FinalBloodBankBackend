﻿using Microsoft.EntityFrameworkCore;
using StockMicroservice.Models;

namespace StockMicroservice.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Stock> Stocks { get; set; }  
    }
}
