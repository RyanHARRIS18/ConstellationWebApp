using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConstellationWebApp.Data;
using ConstellationWebApp.Models;

namespace ConstellationWebApp.Controllers
{
    public class UserProjectsController : Controller
    {
        private readonly ConstellationWebAppContext _context;

        public UserProjectsController(ConstellationWebAppContext context)
        {
            _context = context;
        }

        // GET: UserProjects
        public async Task<IActionResult> Index()
        {
            var constellationWebAppContext = _context.UserProjects.Include(u => u.Project).Include(u => u.User);
            return View(await constellationWebAppContext.ToListAsync());
        }

        // GET: UserProjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProject = await _context.UserProjects
                .Include(u => u.Project)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userProject == null)
            {
                return NotFound();
            }

            return View(userProject);
        }

        // GET: UserProjects/Create
        public IActionResult Create()
        {
            ViewData["Projectid"] = new SelectList(_context.Projects, "ProjectID", "Description");
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Bio");
            return View();
        }

        // POST: UserProjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Projectid")] UserProject userProject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userProject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Projectid"] = new SelectList(_context.Projects, "ProjectID", "Description", userProject.Projectid);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Bio", userProject.UserId);
            return View(userProject);
        }

        // GET: UserProjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProject = await _context.UserProjects.FindAsync(id);
            if (userProject == null)
            {
                return NotFound();
            }
            ViewData["Projectid"] = new SelectList(_context.Projects, "ProjectID", "Description", userProject.Projectid);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Bio", userProject.UserId);
            return View(userProject);
        }

        // POST: UserProjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Projectid")] UserProject userProject)
        {
            if (id != userProject.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userProject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserProjectExists(userProject.UserId))
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
            ViewData["Projectid"] = new SelectList(_context.Projects, "ProjectID", "Description", userProject.Projectid);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Bio", userProject.UserId);
            return View(userProject);
        }

        // GET: UserProjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProject = await _context.UserProjects
                .Include(u => u.Project)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userProject == null)
            {
                return NotFound();
            }

            return View(userProject);
        }

        // POST: UserProjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userProject = await _context.UserProjects.FindAsync(id);
            _context.UserProjects.Remove(userProject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserProjectExists(int id)
        {
            return _context.UserProjects.Any(e => e.UserId == id);
        }
    }
}
