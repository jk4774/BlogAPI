using Microsoft.EntityFrameworkCore;
using BlogEntities;

namespace BlogContext
{
    public class BlogDbContext : DbContext, IBlogDbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }
        // public DbSet<Article> Articles { get; set; }
        // public DbSet<Comment> Comments { get; set; }
        // public DbSet<User> Users { get; set; }
        public System.Data.Entity.IDbSet<Article> IBlogDbContext.Articles { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public System.Data.Entity.IDbSet<Comment> IBlogDbContext.Comments { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public System.Data.Entity.IDbSet<User> IBlogDbContext.Users { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}