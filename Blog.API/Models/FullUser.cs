using System.Collections.Generic;

namespace Blog.API.Models
{

    public class FullUser
    {
        public User User { get; set; }
        public Dictionary<Article, List<Comment>> Articles { get; set; } 
    }

}