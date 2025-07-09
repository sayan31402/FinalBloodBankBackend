using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace PersonMicroservice.Models
{
    public class Donor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DonationId { get; set; }  // Primary Key

        [ForeignKey("Person")]  // Foreign Key to Person table
        [Required] public int PersonId { get; set; }  // Foreign Key to Person table
        [Required] public DateTime DonationDateTime { get; set; }  // DateTime of donation
        [Required] public int Quantity { get; set; }  // In ml / bags


        // Optional fields for updating data
        public int? RBCCount { get; set; }  // Optional, use when updating data
        public int? WBCCount { get; set; }  // Optional, use when updating data
        public int? PlateletCount { get; set; }  // Optional, use when updating data


        // Navigation property
        public Person Person { get; set; }  // Navigation property to Person table
    
    }
}
