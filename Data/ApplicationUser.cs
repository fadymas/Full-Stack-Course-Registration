using Microsoft.AspNetCore.Identity;
namespace WithAuthintication.Data
{
    public class ApplicationUser : IdentityUser
    {
        // Add additional properties here
        public Client Client { get; set; }
    }
}