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
    }
}