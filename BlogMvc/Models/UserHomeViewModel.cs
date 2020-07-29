using System.Collections.Generic;
using BlogEntities;

namespace BlogMvc.Models
{
    public class UserHomeViewModel 
    {
        public User User { get; set; }
        public bool HasErrors { get; set; } = false;
        public IEnumerable<string> Errors { get; set; }
    }
}