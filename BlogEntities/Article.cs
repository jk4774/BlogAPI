using System;
using System.ComponentModel.DataAnnotations;

namespace BlogEntities
{
    public class Article
    {
        [Key]
        public int Id { get; set; } 
        public int UserId { get; set; }
        public string Author { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}