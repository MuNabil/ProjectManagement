using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class DateRangeDto
    {
        [Required]
        public DateTime FromStartDate { get; set; }

        [Required]
        public DateTime ToEndDate { get; set; }
    }
}
