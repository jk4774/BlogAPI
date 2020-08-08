using System.ComponentModel.DataAnnotations;

namespace BlogMvc.Models
{
    public class PasswordViewModel 
    {
        [Required]
        [StringLength(128, MinimumLength = 8, ErrorMessage = "Password length must be minimum 8 and can be maximum 128")]
        public string Old { get; set; }
        [Required]
        [StringLength(128, MinimumLength = 8, ErrorMessage = "Password length must be minimum 8 and can be maximum 128")]
        public string New { get; set; }
    }
}