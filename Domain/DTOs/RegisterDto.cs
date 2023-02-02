using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class RegisterDto
    {
        [MaxLength(100)]
        public string UserName { get; set; }

        [MaxLength(250)]
        public string Password { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }
    }
}
