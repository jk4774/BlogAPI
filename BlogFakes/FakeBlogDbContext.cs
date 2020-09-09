using BlogContext;
using BlogData.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogFakes
{
    public class FakeBlogDbContext : IBlogDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        
        public virtual int SaveChanges()
        {
            return 1;
        }
    }
}