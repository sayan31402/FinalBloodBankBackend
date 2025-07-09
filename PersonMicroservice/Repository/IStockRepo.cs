using PersonMicroservice.Models;

namespace PersonMicroservice.Repository
{
    public interface IStockRepo
    {
        Task<IEnumerable<Stock>> GetAllStocks();
        Task<Stock> GetStockByBloodGroup(string bloodGroup);
        Task<bool> CreateStock(Stock stock);
        Task<bool> UpdateStock(string bloodGroup, Stock stock);
        Task<bool> DeleteStock(string bloodGroup);
    }
}
