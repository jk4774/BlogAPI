using Microsoft.EntityFrameworkCore;
using BlogEntities;
using System.Data.Entity;

namespace BlogContext
{
    public interface IBlogDbContext 
    {
        DbSet<Article> Articles { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<User> Users { get; set; }
        int SaveChanges();
    }
}