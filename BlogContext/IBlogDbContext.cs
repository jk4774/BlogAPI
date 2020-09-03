using BlogEntities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlogContext
{
    public interface IBlogDbContext 
    {
        DbSet<Article> Articles { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<User> Users { get; set; }

        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}