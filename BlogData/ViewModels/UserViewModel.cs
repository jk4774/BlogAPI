using System.Collections.Generic;
using BlogData.Entities;

namespace BlogData.ViewModels
{
    public class UserViewModel 
    {
        public User User { get; set; }
        public IEnumerable<ArticleViewModel> ArticleViewModel { get; set; }
    }
}