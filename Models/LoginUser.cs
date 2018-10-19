using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Belt.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage = "Missing email address")]
        public string LogEmail {get; set;}

        [Required(ErrorMessage = "Password can't be empty")]
        public string LogPassword {get; set;}
    }
}