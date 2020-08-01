using System.ComponentModel.DataAnnotations;

namespace BlogEntities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(128, MinimumLength = 8, ErrorMessage = "Password length must be minimum 8 and can be maximum 128")]
        public string Password { get; set; }
    }
}