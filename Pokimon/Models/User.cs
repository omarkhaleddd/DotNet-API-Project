using System.ComponentModel.DataAnnotations;

namespace Pokimon.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength =2)]
        public string? FirstName { get; set; }

        [EmailAddress(ErrorMessage = "Enter valid email")]
        public string Email { get; set; }
        [Range(10, 99, ErrorMessage ="you must be between 10 - 99 years old")]
        public int? Age { get; set; }
        public int? Role { get; set; } = 0;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

    }
}
