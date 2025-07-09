using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonMicroservice.Models
{
    public class Receiver
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReceiverId { get; set; }  // Primary Key

        [Required][ForeignKey("Person")] public int PersonId { get; set; }  // Foreign Key to Person table
        [Required] public DateTime ReceiverDateTime { get; set; }  // DateTime of receiver
        [Required] public int Quantity { get; set; }  // In ml / bags
        [Required] public string HospitalName { get; set; }  // Name of the hospital where blood is received


        // Navigation property
        public Person Person { get; set; }

    }
}
