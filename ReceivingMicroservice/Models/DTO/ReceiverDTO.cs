using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ReceiverMicroservice.Models.DTO
{
    public class ReceiverDTO
    {
        [Required][ForeignKey("Person")] public int PersonId { get; set; }  // Foreign Key to Person table
        [Required] public DateTime ReceiverDateTime { get; set; }  // DateTime of receiver
        [Required] public int Quantity { get; set; }  // In ml / bags
        [Required] public string HospitalName { get; set; }  // Name of the hospital where blood is received

        //public DateTime CreatedAt { get; set; }  // DateTime of AddReceiver
        //public DateTime UpdatedAt { get; set; }  // DateTime of UpdateReceiver


        // Navigation property
        public Person Person { get; set; }

    }
}

