﻿using System.ComponentModel.DataAnnotations;

namespace LearnJWT.Model
{
    public class Users
    {
        [Key]
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Name{ get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; } 
    }
}
