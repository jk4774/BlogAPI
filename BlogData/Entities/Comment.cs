using System;
using System.ComponentModel.DataAnnotations;

namespace BlogData.Entities
{
    public class Comment 
    {
        [Key]
        public int Id { get; set; } 
        public int UserId { get; set; }
        public int ArticleId { get; set; }
        public string Author { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}