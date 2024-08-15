using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WithAuthintication.Data;
using WithAuthintication.Models;

namespace WithAuthintication.Controllers
{
    [Authorize("AdminRole")]


    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ClientsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [AllowAnonymous]

        public async Task<IActionResult> Details()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                ViewBag.UserName = user.UserName;
            }



            var client = await _context.clients
                .Include(c => c.ClientCourses)
                    .ThenInclude(cc => cc.Course)
                .FirstOrDefaultAsync(m => m.UserId == user.Id);

            if (client == null)
            {
                return NotFound();
            }

            var viewModel = new ClientCoursesViewModel
            {
                ClientId = client.UserId,
                ClientName = user.UserName,
                Courses = client.ClientCourses.Select(cc => cc.Course).ToList()
            };
            if (viewModel.Courses.Count == 0)
            {
                HttpContext.Session.SetString("no", "false");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(viewModel);

            }

        }
        public async Task<IActionResult> Index()
        {
            var clients = await _context.clients
            .Include(c => c.Department) // Include the Department entity
            .ToListAsync();

            

            return View(clients);
        }
        public async Task<IActionResult> ManageCourses(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == id);
            ViewData["user"] = user.UserName;

            var client = await _context.clients
                .Include(c => c.Department)
                .Include(c => c.ClientCourses)
                .ThenInclude(cc => cc.Course)
                .FirstOrDefaultAsync(m => m.UserId == id);

            if (client == null)
            {
                return NotFound();
            }

            ViewData["Courses"] = new SelectList(_context.courses.Where(c => c.DepartmentId == client.DepartmentId), "CourseId", "Name");
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCourse(string clientId, int courseId)
        {
            if (string.IsNullOrEmpty(clientId) || courseId <= 0)
            {
                return BadRequest("Invalid client or course ID.");
            }

            var clientCourse = await _context.ClientCourses
                .FirstOrDefaultAsync(cc => cc.ClientId == clientId && cc.CourseId == courseId);

            if (clientCourse != null)
            {
                ModelState.AddModelError(string.Empty, "This course has already been added to the client.");
                return RedirectToAction(nameof(ManageCourses), new { id = clientId });
            }

            var newClientCourse = new ClientCourse
            {
                ClientId = clientId,
                CourseId = courseId
            };

            _context.ClientCourses.Add(newClientCourse);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageCourses), new { id = clientId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCourse(string clientId, int courseId)
        {
            var clientCourse = await _context.ClientCourses
                .FirstOrDefaultAsync(cc => cc.ClientId == clientId && cc.CourseId == courseId);

            if (clientCourse != null)
            {
                _context.ClientCourses.Remove(clientCourse);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ManageCourses), new { id = clientId });
        }



        // GET: Clients1/Create
        public IActionResult Create()
        {
            ViewData["UserName"] = new SelectList(_context.Users, "UserName", "UserName");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id");
            ViewData["DepartmentName"] = new SelectList(_context.Departments, "Name", "Name");
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return View();
        }

        // POST: Clients1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string DepartmentName, [Bind("UserId,data_of_birth,DepartmentId")] Client client)
        {
            var department = await _context.Departments
                   .FirstOrDefaultAsync(d => d.Name == DepartmentName);
            var user = await _context.Users.FirstOrDefaultAsync(d => d.UserName == client.UserId);
            if (department == null)
            {
                // Handle the case where the department is not found
                ModelState.AddModelError(string.Empty, "Department not found.");
                ViewData["DepartmentName"] = new SelectList(_context.Departments, "Name", "Name");
                ViewData["UserName"] = new SelectList(_context.Users, "UserName", "UserName");
                ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id");
                ViewData["DepartmentName"] = new SelectList(_context.Departments, "Name", "Name");
                ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
                return View(client);
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var adminClaim = claims.FirstOrDefault(c => c.Type == "Admin" && c.Value == "Admin");
            if (adminClaim != null)
            {
                ModelState.AddModelError(string.Empty, "The user already has the 'Admin' claim and cannot be updated.");
                ViewData["UserName"] = new SelectList(_context.Users, "UserName", "UserName");
                ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id");
                ViewData["DepartmentName"] = new SelectList(_context.Departments, "Name", "Name");
                ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
                return View();
            }
            client.UserId = user.Id;

            var student = await _context.clients.FirstOrDefaultAsync(d => d.UserId == client.UserId);
            if (student != null)
            {
                ModelState.AddModelError(string.Empty, "Student was Found");
                ViewData["DepartmentName"] = new SelectList(_context.Departments, "Name", "Name");
                ViewData["UserName"] = new SelectList(_context.Users, "UserName", "UserName");
                ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id");
                ViewData["DepartmentName"] = new SelectList(_context.Departments, "Name", "Name");
                ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
                return View(client);
            }

            // Assign the DepartmentId to the Course entity
            client.DepartmentId = department.Id;
            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Add(client);
                    user.EmailConfirmed = true;
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                }
            }
            ViewData["UserName"] = new SelectList(_context.Users, "UserName", "UserName");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id");
            ViewData["DepartmentName"] = new SelectList(_context.Departments, "Name", "Name");
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return RedirectToAction(nameof(Index));
        }

        // GET: Clients1/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["User"] = user;
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", client.DepartmentId);
            ViewData["DepartmentName"] = new SelectList(_context.Departments, "Name", "Name");
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", client.UserId);
            return View(client);
        }

        // POST: Clients1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string DepartmentName, [Bind("UserId,data_of_birth,DepartmentId")] Client client)
        {
            var department = await _context.Departments
                   .FirstOrDefaultAsync(d => d.Name == DepartmentName);


            if (department == null)
            {
                // Handle the case where the department is not found
                ModelState.AddModelError(string.Empty, "Department not found.");
                ViewData["DepartmentName"] = new SelectList(_context.Departments, "Name", "Name");
                return View(client);
            }



            var existingClient = await _context.clients
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.UserId == id);

            if (existingClient == null)
            {
                return NotFound();
            }

            // Check if the department is being changed
            if (existingClient.DepartmentId != department.Id)
            {
                // Check if the user has any courses
                var userCourses = await _context.ClientCourses
                    .Where(c => c.ClientId == client.UserId)
                    .ToListAsync();

                if (userCourses.Any())
                {
                    // Add a validation error if the user has courses
                    ModelState.AddModelError(string.Empty, "Cannot change department because the user has courses.");
                    ViewData["DepartmentName"] = new SelectList(_context.Departments, "Name", "Name");
                    return RedirectToAction(nameof(Index));
                }

                // Assign the new DepartmentId to the Client entity

            }
            if (id != client.UserId)
            {
                return NotFound();
            }
            client.DepartmentId = department.Id;
            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", client.DepartmentId);
            ViewData["DepartmentName"] = new SelectList(_context.Departments, "Name", "Name");
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", client.UserId);
            return View(client);
        }

        // GET: Clients1/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.clients
                .Include(c => c.Department)

                .FirstOrDefaultAsync(m => m.UserId == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var client = await _context.clients.FindAsync(id);
            if (client != null)
            {
                var uss = await _userManager.Users.FirstOrDefaultAsync(d => d.Id == id);
                uss.EmailConfirmed = false;
                _context.clients.Remove(client);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(string id)
        {
            return _context.clients.Any(e => e.UserId == id);
        }
    }
}
