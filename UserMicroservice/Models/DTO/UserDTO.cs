using System.ComponentModel.DataAnnotations;

namespace UserMicroservice.Models.DTO
{
    public class UserCreateDTO
    {

        [Required, MaxLength(100)]
        public string UserName { get; set; }

        [Required, MinLength(6), MaxLength(16)]
        public string Password { get; set; } // Hashed

        [Required]
        [EnumDataType(typeof(UserRole))]
        public string Role { get; set; } // As string for simpler transfer
    }

    public class UserUpdateDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; } // Hashed
    }

    public class UserGetDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; } // Hashed
        public string Role { get; set; }
    }
}
