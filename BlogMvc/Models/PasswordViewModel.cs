using System.ComponentModel.DataAnnotations;

namespace BlogMvc.Models
{
    public class PasswordViewModel 
    {
        [Required]
        public string Old { get; set; }
        [Required]
        public string New { get; set; }
    }
}