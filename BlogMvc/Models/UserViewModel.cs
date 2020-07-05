using System.Collections.Generic;
using BlogEntities;

namespace BlogMvc.Models
{
    public class UserViewModel 
    {
        public User User { get; set; }
        public IList<Article> Articles { get; set; }
    }
}