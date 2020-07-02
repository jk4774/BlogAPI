using System;
using System.ComponentModel.DataAnnotations;

namespace BlogEntities
{
    public class Article
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        [EmailAddress]
        public string AuthorEmail { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}