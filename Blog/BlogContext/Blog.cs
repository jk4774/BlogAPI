using Microsoft.EntityFrameworkCore;
using BlogEntities;

namespace BlogContext
{
    public class Blog : DbContext
    {
        public Blog(DbContextOptions<Blog> options) : base (options) { }
        public DbSet<Article> Articles { get; set; } 
        public DbSet<Comment> Comments { get; set; } 
        public DbSet<User> Users { get; set; } 
    }
}