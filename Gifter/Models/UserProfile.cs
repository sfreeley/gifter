﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gifter.Models
{
    public class UserProfile
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        //this property can be null in database
        public string ImageUrl { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

    }
}
