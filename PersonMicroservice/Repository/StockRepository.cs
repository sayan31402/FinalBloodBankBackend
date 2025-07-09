using Microsoft.EntityFrameworkCore;
using PersonMicroservice.Data;
using PersonMicroservice.Models;

namespace PersonMicroservice.Repository
{
    public class StockRepository : IStockRepo
    {
        private readonly PersonDbContext _context;
        public StockRepository(PersonDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateStock(Stock stock)
        {
            var result = await _context.Stocks.FirstOrDefaultAsync(c => c.BloodGroup == stock.BloodGroup);
            if (result != null) return false; // Stock already exists

            // Set CreatedAt and UpdatedAt to Indian Standard Time (IST)
            var indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            stock.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, indiaTimeZone);
            stock.UpdatedAt = stock.CreatedAt; // Initially, UpdatedAt is the same as CreatedAt

            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteStock(string bloodGroup)
        {
            var result = await _context.Stocks.FirstOrDefaultAsync(c => c.BloodGroup == bloodGroup);
            if (result == null)
            {
                return false; // Stock not found
            }
            _context.Stocks.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Stock>> GetAllStocks()
        {
            return await _context.Stocks.ToListAsync();
        }

        public async Task<Stock> GetStockByBloodGroup(string bloodGroup)
        {
            var result = await _context.Stocks.FirstOrDefaultAsync(c => c.BloodGroup == bloodGroup);
            return result;
        }

        public async Task<bool> UpdateStock(string bloodGroup, Stock stock)
        {
            var result = await _context.Stocks.FirstOrDefaultAsync(c => c.BloodGroup == bloodGroup);
            if (result == null) return false; // Stock not found

            result.Quantity = result.Quantity + stock.Quantity;
            
            // Set UpdatedAt to Indian Standard Time (IST)
            var indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            result.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, indiaTimeZone);

            var state = _context.Entry(result).State;
            Console.WriteLine($"Entity State: {state}");
            await _context.SaveChangesAsync();

            //await _context.SaveChangesAsync();
            return true; // Stock updated successfully
        }
    }
}
