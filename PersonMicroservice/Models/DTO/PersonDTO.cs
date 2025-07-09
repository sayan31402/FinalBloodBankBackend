using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PersonMicroservice.Models.DTO
{
    public class PersonDTO
    {
        [Required][MaxLength(100)] public string Name { get; set; }
        [Required] public string Gender { get; set; }
        [Required] public string BloodGroup { get; set; }
        [Required] public string Address { get; set; }
        [Required, MaxLength(10), MinLength(10)] public string PhoneNumber { get; set; }
        [Required, EmailAddress] public string Email { get; set; }

        public DateTime CreatedAt { get; set; }  // DateTime of AddPerson
        public DateTime UpdatedAt { get; set; }  // DateTime of UpdatePerson
        public int PersonId { get; set; }
    }
}
