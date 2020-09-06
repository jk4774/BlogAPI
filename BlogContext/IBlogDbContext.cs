using System.Data.Entity;
using BlogEntities;

namespace BlogContext
{
    public interface IBlogDbContext 
    {
        IDbSetExtended<Article> Articles { get; set; }
        IDbSetExtended<Comment> Comments { get; set; }
        IDbSetExtended<User> Users { get; set; }
        int SaveChanges();
    }
}