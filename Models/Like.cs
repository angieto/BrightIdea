using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Belt.Models
{
    public class Like
    {
        public int LikeId {get; set;}

        // navigation properties
        public int UserId {get; set;}
        public User User {get; set;}

        public int PostId {get; set;}
        public Post Post {get; set;}

        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;
    }
}