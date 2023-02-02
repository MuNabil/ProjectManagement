using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Project
    {
        public int? Id { get; set; }

        [Required, MaxLength(180)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? DeveloperId { get; set; }

        public Developer? Developer { get; set; }

    }
}
