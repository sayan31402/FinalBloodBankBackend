using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserMicroservice.Models
{
    public enum UserRole
    {
        ADMIN,
        EMPLOYEE
    }
    public class User
    {
        [Key] public int UserId { get; set; }
        [Required, MaxLength(100)] public string UserName { get; set; }
        [Required] public string Password { get; set; }       // Hashed

        [Required]
        [EnumDataType(typeof(UserRole))]
        [Column(TypeName = "varchar(20)")]
        public UserRole Role { get; set; }       // ADMIN, EMPLOYEE

        public DateTime? CreatedAt { get; set; }          // DateTime of AddUser
        public DateTime? UpdatedAt { get; set; }          // DateTime of UpdateUser

    }
}
