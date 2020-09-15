﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BullkyBook.Models
{
    public class CoverType
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        [Display(Name = "Cover Type")]
        public string Name { get; set; }
    }
}
