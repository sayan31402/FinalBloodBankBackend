using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using System.Linq;

using DonationMicroservice;
using DonationMicroservice.Models;
using DonationMicroservice.Data;
using Microsoft.Identity.Client;


namespace DonationMicroservice.Repository
{
    public class DonationRepository : IDonationRepository
    {
        private readonly DonationDbContext _context;
        public DonationRepository(DonationDbContext context) { _context = context; }

        //--------------------------------------------------------------------------
        //General Operations
        //----Get All Donors----------------------------------------------------------------------
        public async Task<IEnumerable<Donor>> GetAllDonors()
        {
            var Donors = await _context.Donors.ToListAsync();

            return Donors;
        }

        //----Get Donor By Id----------------------------------------------------------------------
        public async Task<Donor?> GetDonorById(int Id) 
        {
            var donor = await _context.Donors.FirstOrDefaultAsync(p => p.DonationId == Id);
            //check if Donor exist in the database
            if (donor == null) {return null;}
            //Else Output
            return donor;
        }

        //--------------------------------------------------------------------------

        //==========================================================================





        //--------------------------------------------------------------------------
        //Authorised Operations
        //----Add Donor----------------------------------------------------------------------
        public async Task<bool> AddDonor(Donor donor) 
        {
            try
            {
                //----Exceptions

                //----*************
                //Adding to the DataBase
                var status = await _context.Donors.AddAsync(donor);
                bool res = status.State == EntityState.Added;
                await _context.SaveChangesAsync();

                return res;
            }

            //----Catch ****
            catch (Exception ex) { return false; }
        }

        //----Update Donor----------------------------------------------------------------------
        public async Task<bool> UpdateDonor(Donor DonorInput, int Id)
        {
            try
            {
                var existingDonor = await _context.Donors.FirstOrDefaultAsync(p => p.DonationId == Id);
                //Checking if Donor Exist
                if (existingDonor == null) { return false; }

                //Converting DonorDTO (Input Structure) to Donor model(DataBase Structure)
                //existingDonor.DonationId = DonorInput.DonationId;
                existingDonor.PersonId = DonorInput.PersonId;
                existingDonor.DonationDateTime = DonorInput.DonationDateTime;
                existingDonor.Quantity = DonorInput.Quantity;
                existingDonor.RBCCount = DonorInput.RBCCount;
                existingDonor.WBCCount = DonorInput.WBCCount;
                existingDonor.PlateletCount = DonorInput.PlateletCount;

                //Updating to the DataBase
                await _context.SaveChangesAsync();
                return true;
            }

            //----Catch ***
            catch (Exception ex) { return false; }
        }

        //----Delete Donor----------------------------------------------------------------------
        public async Task<bool> DeleteDonor(int Id)
        {
            try
            {
                var Donor = await _context.Donors.FirstOrDefaultAsync(p => p.DonationId == Id);
                //Checking if Donor Exist
                if (Donor == null) return false;
                //Else Deleting from the DataBase
                var status = _context.Donors.Remove(Donor);
                bool res = status.State == EntityState.Deleted;
                await _context.SaveChangesAsync();

                return res;
            }

            //----Catch ****
            catch (Exception ex) { return false; }
        }

        //--------------------------------------------------------------------------

        //==========================================================================

    }
}
