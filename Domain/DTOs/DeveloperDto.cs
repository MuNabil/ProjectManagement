using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class DeveloperDto
    {
        [Required, MaxLength(150)]
        public string Name { get; set; }
        public string? Type { get; set; }
        public int? Followers { get; set; }
    }
}
