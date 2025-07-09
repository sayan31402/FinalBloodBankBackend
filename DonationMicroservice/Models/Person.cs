namespace DonationMicroservice.Models
{
    public class Person
    {
        public int PersonId { get; set; }  // Primary Key
        public string Name { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public DateTime CreatedAt { get; set; }  // DateTime of AddPerson
        public DateTime UpdatedAt { get; set; }  // DateTime of UpdatePerson

    }
}
