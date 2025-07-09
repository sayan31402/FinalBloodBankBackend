using PersonMicroservice.Models;

namespace PersonMicroservice.Repository
{
    public interface IReceiverRepository
    {
        Task<IEnumerable<Receiver>> GetAllReceivers();
        Task<Receiver?> GetReceiverById(int Id);
        //--------------------------------------------------------------
        Task<bool> AddReceiver(Receiver Receiver);
        Task<bool> UpdateReceiver(Receiver Receiver, int Id);
        Task<bool> DeleteReceiver(int Id);
    }
}
