using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonMicroservice.Models
{
    public class Stock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string BloodGroup { get; set; }  // Primary Key
        public uint Quantity { get; set; }
        public DateTime? CreatedAt { get; set; }  // DateTime of AddStock
        public DateTime? UpdatedAt { get; set; }  // DateTime of UpdateStock

    }
}
