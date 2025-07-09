using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using System.Linq;

using ReceiverMicroservice.Models;
using ReceiverMicroservice.Data;


namespace ReceiverMicroservice.Repository
{
    public class ReceiverRepository : IReceiverRepository
    {
        private readonly ReceiverDbContext _context;
        public ReceiverRepository(ReceiverDbContext context) { _context = context; }

        //--------------------------------------------------------------------------
        //General Operations
        //----Get All Receivers----------------------------------------------------------------------
        public async Task<IEnumerable<Receiver>> GetAllReceivers()
        {
            var Receivers = await _context.Receivers.ToListAsync();

            return Receivers;
        }

        //----Get Receiver By Id----------------------------------------------------------------------
        public async Task<Receiver?> GetReceiverById(int Id) 
        {
            var receiver = await _context.Receivers.FirstOrDefaultAsync(p => p.ReceiverId == Id);
            //check if Receiver exist in the database
            if (receiver == null) {return null;}
            //Else Output
            return receiver;
        }


        //--------------------------------------------------------------------------

        //==========================================================================





        //--------------------------------------------------------------------------
        //Authorised Operations
        //----Add Receiver----------------------------------------------------------------------
        public async Task<bool> AddReceiver(Receiver receiver) 
        {
            try
            {
                //----Exceptions

                //----*************
                //Adding to the DataBase
                var status = await _context.Receivers.AddAsync(receiver);
                bool res = status.State == EntityState.Added;
                await _context.SaveChangesAsync();

                return res;
            }

            //----Catch ****
            catch (Exception ex) { return false; }
        }

        //----Update Receiver----------------------------------------------------------------------
        public async Task<bool> UpdateReceiver(Receiver ReceiverInput, int Id)
        {
            try
            {
                var existingReceiver = await _context.Receivers.FirstOrDefaultAsync(p => p.ReceiverId == Id);
                //Checking if Receiver Exist
                if (existingReceiver == null) { return false; }

                //Converting ReceiverDTO (Input Structure) to Receiver model(DataBase Structure)
                //existingReceiver.ReceiverId = ReceiverInput.ReceiverId;
                existingReceiver.PersonId = ReceiverInput.PersonId;
                existingReceiver.ReceiverDateTime = ReceiverInput.ReceiverDateTime;
                existingReceiver.Quantity = ReceiverInput.Quantity;
                existingReceiver.HospitalName = ReceiverInput.HospitalName;
                
                //Updating to the DataBase
                await _context.SaveChangesAsync();
                return true;
            }

            //----Catch ***
            catch (Exception ex) { return false; }
        }

        //----Delete Receiver----------------------------------------------------------------------
        public async Task<bool> DeleteReceiver(int Id)
        {
            try
            {
                var Receiver = await _context.Receivers.FirstOrDefaultAsync(p => p.ReceiverId == Id);
                //Checking if Receiver Exist
                if (Receiver == null) return false;
                //Else Deleting from the DataBase
                var status = _context.Receivers.Remove(Receiver);
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
