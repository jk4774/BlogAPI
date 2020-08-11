using System.Collections.Generic;
using BlogEntities;

namespace BlogMvc.Models
{
    public class ArticleViewModel
    {
         public Article Article { get; set; }
         public IEnumerable<Comment> Comments { get; set; }
    }
}