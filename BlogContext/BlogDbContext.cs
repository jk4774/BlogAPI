using Microsoft.EntityFrameworkCore;
using BlogEntities;

namespace BlogContext
{
    public class BlogDbContext : DbContext, IBlogDbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }

        public IDbSetExtended<User> Users  { get; set; }
        public IDbSetExtended<Article> Articles { get; set; }
        public IDbSetExtended<Comment> Comments { get; set; }
    }
}