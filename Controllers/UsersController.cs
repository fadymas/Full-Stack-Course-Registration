using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WithAuthintication.Data; // Update with your actual namespace
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

[Authorize("AdminRole")]
public class UsersController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;

    public UsersController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    // GET: Users
    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.ToListAsync();
        return View(users);
    }

    // GET: Users/Add
    public IActionResult Add()
    {
        return View();
    }

    // POST: Users/Add
    [HttpPost]
    public async Task<IActionResult> Add(string email, string password,string username)
    {
        IdentityResult result = IdentityResult.Failed();
        if (ModelState.IsValid)
        {
            
           
            var existingEmail = _userManager.FindByEmailAsync(email);
            if (existingEmail.Result != null)
            {
                var error = new IdentityError
                {
                    Code = "This Email Was Used",
                    Description = "This Email Was Used"
                };
                result = IdentityResult.Failed(error);

            }
            else
            {
                try
                {
                    var user = new IdentityUser { UserName = username, Email = email };
                    
                    result = await _userManager.CreateAsync(user, password);
                }
                catch (Exception ex)
                {
                    var error = new IdentityError
                    {
                        Code = "This Name Was Used",
                        Description = "This Name Was Used"
                    };
                    result = IdentityResult.Failed(error);
                }
                
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                
            }
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View();
    }

    // POST: Users/Delete/{id}
    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            return RedirectToAction(nameof(Index));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return RedirectToAction(nameof(Index));
    }
}
