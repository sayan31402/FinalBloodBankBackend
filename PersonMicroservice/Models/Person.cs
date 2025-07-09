using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonMicroservice.Models
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonId { get; set; }  // Primary Key
        [Required][MaxLength(100)] public string Name { get; set; }
        [Required] public string Gender { get; set; }
        [Required] public string BloodGroup { get; set; }
        [Required] public string Address { get; set; }
        [Required, MaxLength(10), MinLength(10)] public string PhoneNumber { get; set; }
        [Required, EmailAddress] public string Email { get; set; }

        public DateTime? CreatedAt { get; set; }  // DateTime of AddPerson
        public DateTime? UpdatedAt { get; set; }  // DateTime of UpdatePerson


        // Navigation Properties
        public ICollection<Donor>? Donors { get; set; } = new List<Donor>();
        public ICollection<Receiver>? Receivers { get; set; } = new List<Receiver>(); 
    }
}
