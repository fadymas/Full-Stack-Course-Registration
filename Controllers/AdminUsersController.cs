using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WithAuthintication.Data; // Update with your actual namespace
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
[Authorize("AdminRole")]
public class AdminUsersController : Controller
{
     private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;   
       

    public AdminUsersController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    // GET: AdminUsers
    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.ToListAsync();
        var adminUsers = new List<IdentityUser>();

        foreach (var user in users)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            if (claims.Any(c => c.Type == "Admin" && c.Value == "Admin"))
            {
                adminUsers.Add(user);
            }
        }

        return View(adminUsers);
    }

    // GET: AdminUsers/Add
    public IActionResult Add()
    {
        return View();
    }

    // POST: AdminUsers/Add
    [HttpPost]
    public async Task<IActionResult> Add(string email)
    {
        IdentityResult result = IdentityResult.Failed();

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "User not found.");
            return View();
        }

        

        var client = await _context.clients.FirstOrDefaultAsync(d => d.UserId == user.Id);
        if (client != null)
        {
            var error = new IdentityError
            {
                Code = "This Is a Client",
                Description = "This Is a Client"
            };
            result = IdentityResult.Failed(error);
            foreach (var esrror in result.Errors)
            {
                ModelState.AddModelError(string.Empty, esrror.Description);
            }
            return View();
        }

        var claims = await _userManager.GetClaimsAsync(user);
        var adminClaim = claims.FirstOrDefault(c => c.Type == "Admin" && c.Value == "Admin");
        if (adminClaim != null)
        {
            ModelState.AddModelError(string.Empty, "The user already has the 'Admin' claim and cannot be updated.");
            return View();
        }

        result = await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Admin", "Admin"));
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }
        if (result.Succeeded)
        {
            user.EmailConfirmed = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View();
    }

    // GET: AdminUsers/Edit/{id}
    public async Task<IActionResult> Edit(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // POST: AdminUsers/Edit/{id}
    [HttpPost]
    public async Task<IActionResult> Edit(string id, string email)
    {
        var user = await _userManager.FindByIdAsync(id);
        var usere = await _userManager.FindByEmailAsync(email);
        if (user == null || usere != null)
        {
            return RedirectToAction(nameof(Index));
        }
        var client = _context.clients.FirstOrDefaultAsync(d => d.UserId == usere.Id);
        if (client.Result != null)
        {
            return RedirectToAction(nameof(Index));
        }

        // Check if the user has the "Admin" claim
        var claims = await _userManager.GetClaimsAsync(usere);
        var adminClaim = claims.FirstOrDefault(c => c.Type == "Admin" && c.Value == "Admin");
        if (adminClaim != null)
        {
            ModelState.AddModelError(string.Empty, "The user already has the 'Admin' claim and cannot be updated.");
            return View(user);
        }

        user.Email = email;
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return RedirectToAction(nameof(Index));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(user);
    }


    // POST: AdminUsers/Delete/{id}
    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var claims = await _userManager.GetClaimsAsync(user);
        var adminClaim = claims.FirstOrDefault(c => c.Type == "Admin" && c.Value == "Admin");
        if (adminClaim != null)
        {
            var result = await _userManager.RemoveClaimAsync(user, adminClaim);
            
            
            if (result.Succeeded)
            {
                user.EmailConfirmed = false;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return RedirectToAction(nameof(Index));
    }
}
