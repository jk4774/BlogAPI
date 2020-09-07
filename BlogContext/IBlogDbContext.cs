using Microsoft.EntityFrameworkCore;
using BlogEntities;

namespace BlogContext
{
    public interface IBlogDbContext 
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        int SaveChanges();
    }
}