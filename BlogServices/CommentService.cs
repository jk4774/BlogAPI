using System.Linq;
using BlogContext;

namespace BlogServices
{
    public class CommentService
    {
        public CommentService()
        {   
        }

        public virtual bool AnyArticleById(IBlogDbContext blogDbContext, int id)
        {
            return blogDbContext.Articles.Any(x => x.Id == id);
        }
    }
}