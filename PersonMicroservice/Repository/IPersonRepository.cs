using PersonMicroservice.Models;

namespace PersonMicroservice.Repository
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAllPersons();
        Task<Person?> GetPersonById(int Id);
        Task<List<Person>> GetPersonByName(string Name);
        Task<List<Person>> GetPersonByBloodGroup(string BloodGroup);
        //--------------------------------------------------------------
        Task<bool> AddPerson(Person Person);
        Task<bool> UpdatePerson(Person Person, int Id);
        Task<bool> DeletePerson(int Id);
    }
}
