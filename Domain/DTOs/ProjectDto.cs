using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class ProjectDto
    {
        [Required, MaxLength(180)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? DeveloperId { get; set; }
    }
}
