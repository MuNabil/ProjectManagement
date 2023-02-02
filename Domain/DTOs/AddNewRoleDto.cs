using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class AddNewRoleDto
    {
        [Required]
        public string RoleName { get; set; }
    }
}
