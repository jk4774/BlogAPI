using System;
using System.ComponentModel.DataAnnotations;

namespace BlogEntities
{
    public class Comment 
    {
        [Key]
        public int Id { get; set; } 
        public int UserId { get; set; }
        public int ArticleId { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}