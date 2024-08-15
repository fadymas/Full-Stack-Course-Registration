using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WithAuthintication;
using WithAuthintication.Data;

namespace WithAuthintication.Controllers
{
    [Authorize("AdminRole")]
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Course.Include(c => c.Department);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Department)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id");
            ViewData["DepartmentName"] = new SelectList(_context.Departments, "Name", "Name");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string DepartmentName, [Bind("CourseId,Name,Description,DepartmentName")] Course course)
        {
            var department = await _context.Departments
                               .FirstOrDefaultAsync(d => d.Name == DepartmentName);
            if (department == null)
            {
                // Handle the case where the department is not found
                ModelState.AddModelError(string.Empty, "Department not found.");
                ViewData["DepartmentName"] = new SelectList(_context.Departments, "Name", "Name");
                return View(course);
            }

            // Assign the DepartmentId to the Course entity
            course.DepartmentId = department.Id;

            if (!ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", course.DepartmentId);
            ViewData["DepartmentName"] = new SelectList(_context.Departments, "Name", "Name");
            return View(course);
        }



        // GET: Courses/Edit/5
        [HttpGet("Courses/Edit/{id:int}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", course.DepartmentId);
            ViewData["DepartmentName"] = new SelectList(_context.Departments, "Name", "Name");
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost("Courses/Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,string DepartmentName, [Bind("CourseId,Name,Description,DepartmentId")] Course course)
        {
            var department = await _context.Departments
                   .FirstOrDefaultAsync(d => d.Name == DepartmentName);
            if (department == null)
            {
                // Handle the case where the department is not found
                ModelState.AddModelError(string.Empty, "Department not found.");
                ViewData["DepartmentName"] = new SelectList(_context.Departments, "Name", "Name");
                return View(course);
            }

            // Assign the DepartmentId to the Course entity
            course.DepartmentId = department.Id;

            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
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
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", course.DepartmentId);
            ViewData["DepartmentName"] = new SelectList(_context.Departments, "Name", "Name");
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Department)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course != null)
            {
                _context.Course.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseId == id);
        }
    }
}
