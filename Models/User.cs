using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Belt.Models
{
    public class User 
    {
        public int UserId {get; set;}

        [Required(ErrorMessage = "User name is required")]
        [MinLength(2, ErrorMessage = "User name should be 2 characters or longer")]
        public string Name {get; set;}

        [Required(ErrorMessage = "Please type your alias")]
        [MinLength(2, ErrorMessage = "Alias should be 2 characters or longer")]
        public string Alias {get; set;}

        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress]
        public string Email {get; set;}

        [Required(ErrorMessage = "Missing password")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters or longer")]
        [DataType(DataType.Password)]
        public string Password {get; set;}

        public List<Post> Posts {get; set;}
        public List<Like> Likes {get; set;}

        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;

        // Won't be mapped to user table
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm {get; set;}
        
    }
}