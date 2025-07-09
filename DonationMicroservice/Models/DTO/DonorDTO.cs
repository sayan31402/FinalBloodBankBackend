using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace DonationMicroservice.Models.DTO
{
    public class DonorDTO
    {
        [ForeignKey("Person")]  // Foreign Key to Person table
        [Required] public int PersonId { get; set; }  // Foreign Key to Person table
        [Required] public DateTime DonationDateTime { get; set; }  // DateTime of donation
        [Required] public int Quantity { get; set; }  // In ml / bags


        // Optional fields for updating data
        public int? RBCCount { get; set; }  // Optional, use when updating data
        public int? WBCCount { get; set; }  // Optional, use when updating data
        public int? PlateletCount { get; set; }  // Optional, use when updating data

        //public DateTime CreatedAt { get; set; }  // DateTime of AddDonation
        //public DateTime UpdatedAt { get; set; }  // DateTime of UpdateDonation


        // Navigation property
        public Person Person { get; set; }  // Navigation property to Person table

    }
}
