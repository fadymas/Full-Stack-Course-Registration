using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WithAuthintication.Data;
using WithAuthintication.Models;

namespace WithAuthintication.Controllers
{
    [Authorize("AdminRole")]

    public class ClientCoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientCoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ClientCourses
        public async Task<IActionResult> Index()
        {
            ViewData["DepartmentName"] = new SelectList(_context.Departments, "Name", "Name");
            var applicationDbContext = _context.ClientCourses.Include(c => c.Client).Include(c => c.Course);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ClientCourses/Details/5
        

        // POST: ClientCourses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string CourseName,[Bind("ClientId,CourseId")] ClientCourse clientCourse)
        {
            var course = await _context.Departments
                               .FirstOrDefaultAsync(d => d.Name == CourseName);
            if (course == null)
            {
                // Handle the case where the department is not found
                ModelState.AddModelError(string.Empty, "Department not found.");
                ViewData["DepartmentName"] = new SelectList(_context.Departments, "Name", "Name");
                return View(course);
            }

            // Assign the DepartmentId to the Course entity
            clientCourse.CourseId = course.Id;
            if (!ModelState.IsValid)
            {
                _context.Add(clientCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.clients, "UserId", "UserId", clientCourse.ClientId);
            ViewData["CourseId"] = new SelectList(_context.courses, "CourseId", "CourseId", clientCourse.CourseId);
            ViewData["CourseName"] = new SelectList(_context.courses, "Name", "Name");
            return View(clientCourse);
        }


        // GET: ClientCourses/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientCourse = await _context.ClientCourses
                .Include(c => c.Client)
                .Include(c => c.Course)
                .FirstOrDefaultAsync(m => m.ClientId == id);
            if (clientCourse == null)
            {
                return NotFound();
            }

            return View(clientCourse);
        }

        // POST: ClientCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var clientCourse = await _context.ClientCourses.FindAsync(id);
            if (clientCourse != null)
            {
                _context.ClientCourses.Remove(clientCourse);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientCourseExists(string id)
        {
            return _context.ClientCourses.Any(e => e.ClientId == id);
        }
    }
}
