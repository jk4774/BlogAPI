// using System.Data.Entity;
// using BlogContext;
// using BlogEntities;
// using Microsoft.EntityFrameworkCore;

// namespace BlogFakes
// {
//     public class FakeBlogDbContext : BlogContext.IBlogDbContext
//     {
//         public FakeDbSet<Article> Articles;
//         public FakeDbSet<Comment> Comments;
//         public FakeDbSet<User> Users;

//         IDbSet<Article> IBlogDbContext.Articles { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
//         IDbSet<Comment> IBlogDbContext.Comments { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
//         IDbSet<User> IBlogDbContext.Users { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

//         public virtual int SaveChanges()
//         {
//             throw new System.NotImplementedException();
//         }
//     }
// }