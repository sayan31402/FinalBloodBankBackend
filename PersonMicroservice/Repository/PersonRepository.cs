using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using System.Linq;

using PersonMicroservice.Models;
using PersonMicroservice.Data;
using System.Net;
using System.Reflection;


namespace PersonMicroservice.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly PersonDbContext _context;
        public PersonRepository(PersonDbContext context) { _context = context; }

        //--------------------------------------------------------------------------
        //General Operations
        //----Get All Persons----------------------------------------------------------------------
        public async Task<IEnumerable<Person>> GetAllPersons()
        {
            var Persons = await _context.Persons.ToListAsync();

            return Persons;
        }

        //----Get Person By Id----------------------------------------------------------------------
        public async Task<Person?> GetPersonById(int Id) 
        {
            var person = await _context.Persons.FirstOrDefaultAsync(p => p.PersonId == Id);
            //check if Person exist in the database
            if (person == null) {return null;}
            //Else Output
            return person;
        }

        //----Get Person By Name----------------------------------------------------------------------
        public async Task<List<Person>> GetPersonByName(string Name)
        {
            List<Person> persons = await _context.Persons.Where(p => p.Name == Name).ToListAsync();
            //check if Person exist in the database
            if (persons == null) { return null; }
            //Else Output
            return persons;
        }

        //----Get Person By BloodGroup----------------------------------------------------------------------
        public async Task<List<Person>> GetPersonByBloodGroup(string BloodGroup)
        {
            List<Person> persons = await _context.Persons.Where(p => p.BloodGroup == BloodGroup).ToListAsync();
            //check if Person exist in the database
            if (persons == null) { return null; }
            //Else Output
            return persons;
        }

        //--------------------------------------------------------------------------

        //==========================================================================





        //--------------------------------------------------------------------------
        //Authorised Operations
        //----Add Person----------------------------------------------------------------------
        public async Task<bool> AddPerson(Person person) 
        {
            try
            {
                //----Exceptions

                //----*************
                //Adding to the DataBase
                var status = await _context.Persons.AddAsync(person);
                bool res = status.State == EntityState.Added;
                await _context.SaveChangesAsync();

                return res;
            }

            //----Catch ****
            catch (Exception ex) { return false; }
        }

        //----Update Person----------------------------------------------------------------------
        public async Task<bool> UpdatePerson(Person PersonInput, int Id)
        {
            try
            {
                var existingPerson = await _context.Persons.FirstOrDefaultAsync(p => p.PersonId == Id);
                //Checking if Person Exist
                if (existingPerson == null) { return false; }

                //Converting PersonDTO (Input Structure) to Person model(DataBase Structure)
                //existingPerson.PersonId = PersonInput.PersonId;
                existingPerson.Name = PersonInput.Name;
                existingPerson.Gender = PersonInput.Gender;
                existingPerson.BloodGroup = PersonInput.BloodGroup;
                existingPerson.Address = PersonInput.Address;
                existingPerson.PhoneNumber = PersonInput.PhoneNumber;
                existingPerson.Email = PersonInput.Email;
                existingPerson.CreatedAt = PersonInput.CreatedAt;
                existingPerson.UpdatedAt = PersonInput.UpdatedAt;
                
                //Updating to the DataBase
                await _context.SaveChangesAsync();
                return true;
            }

            //----Catch ***
            catch (Exception ex) { return false; }
        }

        //----Delete Person----------------------------------------------------------------------
        public async Task<bool> DeletePerson(int Id)
        {
            try
            {
                var Person = await _context.Persons.FirstOrDefaultAsync(p => p.PersonId == Id);
                //Checking if Person Exist
                if (Person == null) return false;
                //Else Deleting from the DataBase
                var status = _context.Persons.Remove(Person);
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
