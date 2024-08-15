using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WithAuthintication.Data;

public class CustomUserStore : UserStore<IdentityUser>
{
    public CustomUserStore(ApplicationDbContext context, IdentityErrorDescriber describer = null)
        : base(context, describer)
    {
    }

    public override Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
    {
        // Customize the query here
        return Users.SingleOrDefaultAsync(u => u.Email == normalizedUserName, cancellationToken);
    }
}
    