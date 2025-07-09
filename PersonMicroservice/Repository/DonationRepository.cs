using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using System.Linq;

using PersonMicroservice;
using PersonMicroservice.Models;
using PersonMicroservice.Data;
using Microsoft.Identity.Client;
using System.Drawing;
using System;


namespace PersonMicroservice.Repository
{
    public class DonationRepository : IDonationRepository
    {
        private readonly PersonDbContext _context;
        public DonationRepository(PersonDbContext context) { _context = context; }

        //--------------------------------------------------------------------------
        //General Operations
        //----Get All Donors----------------------------------------------------------------------
        public async Task<IEnumerable<Donor>> GetAllDonors()
        {
            var Donors = await _context.Donors.Include(d => d.Person).ToListAsync();

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
                //Checking if person exist in database
                if (!(await _context.Persons.AnyAsync(p => p.PersonId == donor.PersonId))) 
                    throw new Exception("Person(donor) doesnot exist in database");

                var person = await _context.Persons.FirstAsync(p => p.PersonId == donor.PersonId);

                if (donor.Person.BloodGroup != person.BloodGroup)
                    throw new Exception("BloodGroup of Person(donor) doesnot match with BloodGrouo in Person's database information");

                donor.Person = person;

                //Adding to the DataBase
                var status = await _context.Donors.AddAsync(donor);

                var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.BloodGroup == person.BloodGroup);
                int Quantity = (int)stock.Quantity;
                stock.Quantity = (uint)(Quantity + donor.Quantity);

                bool res = status.State == EntityState.Added;
                await _context.SaveChangesAsync();

                return res;
            }

            //----Catch ****
            catch (Exception ex) { throw; return false; }
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

                var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.BloodGroup == existingDonor.Person.BloodGroup);
                int Quantity = (int)stock.Quantity;
                stock.Quantity = (uint)(Quantity + DonorInput.Quantity - existingDonor.Quantity);

                //Updating to the DataBase
                await _context.SaveChangesAsync();
                return true;
            }

            //----Catch ***
            catch (Exception ex) { throw;  return false; }
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

                var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.BloodGroup == Donor.Person.BloodGroup);
                int Quantity = (int)stock.Quantity;
                stock.Quantity = (uint)(Quantity - Donor.Quantity);

                await _context.SaveChangesAsync();

                return res;
            }

            //----Catch ****
            catch (Exception ex) { throw;  return false; }
        }

        //--------------------------------------------------------------------------

        //==========================================================================

    }
}
