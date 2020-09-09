using System.Collections.Generic;
using BlogData.Entities;

namespace BlogData.ViewModels
{
    public class ArticleViewModel
    {
         public Article Article { get; set; }
         public IEnumerable<Comment> Comments { get; set; }
    }
}