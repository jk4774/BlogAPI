using BlogContext;

namespace BlogServices
{
    public class ArticleService
    {
        private readonly Blog _blog;
        public ArticleService(Blog blog)
        {
            _blog = blog;
        }
    }
}
