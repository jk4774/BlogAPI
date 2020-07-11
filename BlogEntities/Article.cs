using System;
using System.ComponentModel.DataAnnotations;

namespace BlogEntities
{
    public class Article
    {
        public int Id { get; set; } 
        public int UserId { get; set; }
        public string AuthorEmail { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}