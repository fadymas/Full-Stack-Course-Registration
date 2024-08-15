using System.ComponentModel.DataAnnotations;

namespace WithAuthintication.Models
{
    public class AddAdminViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

}

