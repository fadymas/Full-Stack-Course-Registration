using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WithAuthintication.Models;
using WithAuthintication;
using Microsoft.AspNetCore.Identity;

namespace WithAuthintication.Controllers
{
    public class HomeController : Controller
    {

        private readonly IForm _form;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;



        public HomeController(ILogger<HomeController> logger, IForm form, UserManager<IdentityUser> userManger)
        {
            _logger = logger;
            _form = form;
            _userManager = userManger;
        }

        public IActionResult Index()
            
        {
            ViewBag.no = HttpContext.Session.GetString("no");
            HttpContext.Session.SetString("no", "true");
            return View();
            }

        public IActionResult Privacy()
        {
            return View();
        }

        

        [HttpGet]
        public IActionResult resetPassword()
        {
            if (HttpContext.Session.GetString("VisitedRestPassword") == "true")
            {
                // Redirect to another page if the user has already visited the getEmail page
                return RedirectToAction("Error", "Home");
            }

            // Set the session variable
            HttpContext.Session.SetString("VisitedRestPassword", "true");


            string email = HttpContext.Session.GetString("email");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("GetEmail");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> resetPassword(string newPassword)
            {
            HttpContext.Session.SetString("VisitedRestPassword", "false");

            string email = HttpContext.Session.GetString("email");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("getEmail");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("Email not found");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                return RedirectToAction("confirmresetPassword");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }

        [HttpGet]
        public IActionResult getEmail()
        {
            if (HttpContext.Session.GetString("VisitedGetEmail") == "true")
            {
                // Redirect to another page if the user has already visited the getEmail page
                return RedirectToAction("Error", "Home");
            }

            // Set the session variable
            HttpContext.Session.SetString("VisitedGetEmail", "true");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> getEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Email not found";
                return View();
            }

            HttpContext.Session.SetString("email", email);
            return RedirectToAction("ResetPassword");
        }
    


    public IActionResult confirmresetPassword()
        {
             HttpContext.Session.SetString("VisitedGetEmail", "true");
             
            HttpContext.Session.SetString("VisitedRestPassword", "true");

            return View();
        }

        // Action method to handle the update request
        [HttpPost]
        public JsonResult UpdateVisitedGetEmail(bool VisitedGetEmail)
        {
            // Update the variable as needed
            // For example, you might store it in the session or a database

            HttpContext.Session.SetString("VisitedGetEmail", "false");

            // Return a JSON response
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult UpdateVisitedResetPassword(bool VisitedGetEmail)
        {
            // Update the variable as needed
            // For example, you might store it in the session or a database

            HttpContext.Session.SetString("VisitedRestPassword", "false");

            // Return a JSON response
            return Json(new { success = true });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
