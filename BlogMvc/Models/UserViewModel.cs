using System.Collections.Generic;
using BlogEntities;

namespace BlogMvc.Models
{
    public class UserViewModel 
    {
        public User User { get; set; }
        public IEnumerable<ArticleViewModel> ArticleViewModel { get; set; }
    }
}