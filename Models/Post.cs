using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Belt.Models
{
    public class Post
    {
        public int PostId {get; set;}

        [Required(ErrorMessage = "Idea can't be blank")]
        [MinLength(2, ErrorMessage = "Your idea should be more meaningful than that!")]
        public string PostContent {get; set;}

        public int LikeAmount {get; set;}

        public int UserId {get; set;}
        public User User {get; set;}
        public List<Like> Likes {get; set;}

        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;
    }
}