using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Developer
    {
        public int? Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; }
        public string? Type { get; set; }
        public int? Followers { get; set; }
    }
}
