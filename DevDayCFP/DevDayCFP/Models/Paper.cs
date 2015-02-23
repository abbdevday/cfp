﻿using System;
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
        public string Level { get; set; }

        public bool IsThereLiveCoding { get; set; }

        public string EventName { get; set; }

        public DateTime LastModificationDate { get; set; }
    }
}