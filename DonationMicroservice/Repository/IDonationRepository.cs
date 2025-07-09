using DonationMicroservice.Models;

namespace DonationMicroservice.Repository
{
    public interface IDonationRepository
    {
        Task<IEnumerable<Donor>> GetAllDonors();
        Task<Donor?> GetDonorById(int Id);
        //--------------------------------------------------------------
        Task<bool> AddDonor(Donor Donor);
        Task<bool> UpdateDonor(Donor Donor, int id);
        Task<bool> DeleteDonor(int Id);
    }
}
