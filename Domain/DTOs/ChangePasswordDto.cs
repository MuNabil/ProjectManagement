using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class ChangePasswordDto
    {
        [Required]
        public string OldPassword { get; set; } = string.Empty;
        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
    public class UserChangePasswordDto : ChangePasswordDto
    {
        public string? UserId { get; set; }
    }
}
