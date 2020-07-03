using System;
using System.ComponentModel.DataAnnotations;

namespace BlogEntities
{
    public class Comment 
    {
        [Required]
        public int Id { get; set; } 
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ArticleId { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}