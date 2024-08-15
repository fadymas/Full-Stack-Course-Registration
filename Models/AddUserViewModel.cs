using System.ComponentModel.DataAnnotations;

namespace WithAuthintication.Models
{
    public class AddUserViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string UserName { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }

        [StringLength(100, ErrorMessage = "6 Letters or more", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
