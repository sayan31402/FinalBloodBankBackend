using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using System.Linq;

using PersonMicroservice.Models;
using PersonMicroservice.Data;
using System.Drawing;


namespace PersonMicroservice.Repository
{
    public class ReceiverRepository : IReceiverRepository
    {
        private readonly PersonDbContext _context;
        public ReceiverRepository(PersonDbContext context) { _context = context; }

        //--------------------------------------------------------------------------
        //General Operations
        //----Get All Receivers----------------------------------------------------------------------
        public async Task<IEnumerable<Receiver>> GetAllReceivers()
        {
            var Receivers = await _context.Receivers.Include(r => r.Person).ToListAsync();

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
                //Checking if person exist in database
                if (!(await _context.Persons.AnyAsync(p => p.PersonId == receiver.PersonId)))
                    throw new Exception("Person(receiver) doesnot exist in database");

                var person = await _context.Persons.FirstAsync(p => p.PersonId == receiver.PersonId);

                if (receiver.Person.BloodGroup != person.BloodGroup)
                    throw new Exception("BloodGroup of Person(Receiver) doesnot match with BloodGrouo in Person's database information");

                receiver.Person = person;

                //Adding to the DataBase
                var status = await _context.Receivers.AddAsync(receiver);
                bool res = status.State == EntityState.Added;

                var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.BloodGroup == person.BloodGroup);
                int Quantity = (int)stock.Quantity;
                stock.Quantity = (uint)(Quantity - receiver.Quantity);

                await _context.SaveChangesAsync();

                return res;
            }

            //----Catch ****
            catch (Exception ex) { throw;  return false; }
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

                var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.BloodGroup == existingReceiver.Person.BloodGroup);
                int Quantity = (int)stock.Quantity;
                stock.Quantity = (uint)(Quantity - ReceiverInput.Quantity + existingReceiver.Quantity);

                //Updating to the DataBase
                await _context.SaveChangesAsync();
                return true;
            }

            //----Catch ***
            catch (Exception ex) { throw;  return false; }
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

                var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.BloodGroup == Receiver.Person.BloodGroup);
                int Quantity = (int)stock.Quantity;
                stock.Quantity = (uint)(Quantity + Receiver.Quantity);

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
