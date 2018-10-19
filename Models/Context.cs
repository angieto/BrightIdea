using Microsoft.EntityFrameworkCore;

namespace Belt.Models
{
    public class BeltContext: DbContext 
    {
        public BeltContext(DbContextOptions<BeltContext> options) : base(options) {}

        public DbSet<User> Users {get; set;}
        public DbSet<Post> Posts {get; set;}
        public DbSet<Like> Likes {get; set;}
    }
}