using System;
using System.ComponentModel.DataAnnotations;

namespace DevDayCFP.Models
{
    public class Paper
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Abstract { get; set; }
        [Required]
        public string Justification { get; set; }
        [Required]
        [Range(100, 500)]
        public int Level { get; set; }

        public PaperStatus Status { get; set; }

        public DateTime LastModificationDate { get; set; }
    }
}