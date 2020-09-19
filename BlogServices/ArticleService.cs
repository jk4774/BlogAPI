using System.Collections.Generic;
using System.Linq;
using BlogContext;
using BlogData.Entities;
using BlogData.ViewModels;

namespace BlogServices
{
    public class ArticleService
    {
        public ArticleService()
        {
        }

        public virtual IEnumerable<ArticleViewModel> GetArticleViewModels(IBlogDbContext blogDbContext)
        {
            return blogDbContext.Articles.ToList().Select(article => new ArticleViewModel
            {
                Article = article,
                Comments = blogDbContext.Comments.Where(x => x.ArticleId == article.Id).ToList()
            });
        }

        public virtual void RemoveArticle(IBlogDbContext blogDbContext, Article article)
        {
            blogDbContext.Articles.Remove(article);
            var comments = blogDbContext.Comments.Where(x => x.ArticleId == article.Id).ToList();
            if (comments.Count > 0)
                blogDbContext.Comments.RemoveRange(comments);
            
            blogDbContext.SaveChanges();
        }
    }
}