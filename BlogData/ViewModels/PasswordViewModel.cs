using System.ComponentModel.DataAnnotations;

namespace BlogData.ViewModels
{
    public class PasswordViewModel 
    {
        [Required]
        [StringLength(128, MinimumLength = 8, ErrorMessage = "Password length must be minimum 8")]
        public string Old { get; set; }
        [Required]
        [StringLength(128, MinimumLength = 8, ErrorMessage = "Password length must be minimum 8")]
        public string New { get; set; }
    }
}