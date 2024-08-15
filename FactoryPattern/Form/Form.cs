using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WithAuthintication.Data;

namespace WithAuthintication
{
    class Form : IForm
    {
        private readonly ApplicationDbContext _context;

        public Form(ApplicationDbContext context)
        {
            _context = context;
        }

        public void GetEmail(string email)
        {

            if (_context.Users.Any(u => u.Email == email)) { }
            else
            {
                throw new System.Exception("Email not found");
            }
        }


        public void ResetPassword(string email, string password)
        {
            // Assuming _context is your database context and Users is your DbSet<User>
            var user = _context.Users.SingleOrDefault(u => u.Email == email);

            if (user.PasswordHash == password)
            {
                throw new Exception("The provided password matches the current password.");
            }
            else
            {
                user.PasswordHash = password;
                _context.SaveChanges();
            }
        }

    }
}
