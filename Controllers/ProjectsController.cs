using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConstellationWebApp.Data;
using ConstellationWebApp.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using ConstellationWebApp.ViewModels;
using System.Dynamic;

namespace ConstellationWebApp.Controllers
{
   
    public class ProjectsController : Controller
    {
        private readonly ConstellationWebAppContext _context;
        private readonly IWebHostEnvironment hostingEnvironment;

        public ProjectsController(ConstellationWebAppContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        // Get Projects
        public async Task<IActionResult> Index()
        {
            var viewModel = new ViewModel();
            viewModel.Projects = await _context.Projects
                   .Include(i => i.UserProjects)
                     .ThenInclude(i => i.User)
                   .AsNoTracking()
                   .OrderBy(i => i.CreationDate)
                   .ToListAsync();
            return View(viewModel);
    }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.ProjectID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        private void PopulateAssignedProjectData(Project newProject)
        {
            var allUsers = _context.User;
            var userProjects = new HashSet<int>(newProject.UserProjects.Select(c => c.UserID));
            var viewModel = new List<AssignedProjectData>();
            foreach (var users in allUsers)
            {
                viewModel.Add(new AssignedProjectData
                {
                    UserID = users.UserID,
                    UserName = users.UserName,
                    Assigned = userProjects.Contains(users.UserID)
                });
            }
            ViewData["UsersOfConstellation"] = viewModel;
        }


        // GET: Projects/Create
        public async Task<IActionResult> Create(string SearchString)
        {
            var users = from m in _context.User
                        select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                users = users.Where(s => s.UserName.Contains(SearchString));
            }
            var project = new Project();
            project.UserProjects = new List<UserProject>();
            PopulateAssignedProjectData(project);
            return View();
        }

// POST: Projects/Create
// To protect from overposting attacks, enable the specific properties you want to bind to, for 
// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//implementing the View controller following this tutorial https://www.youtube.com/watch?v=aoxEJii70_I

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectCreateViewModel model, string[] selectedCollaborators)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

/*For Photo Upload to system. Then takes ViewModel properties 
 * and converts to Entity Model properties. Then commits*/

                if (model.Photo != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath + "\\image\\");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                Project newProject = new Project
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    CreationDate = DateTime.Now,
                    PhotoPath = uniqueFileName
                 };
                _context.Add(newProject);
                await _context.SaveChangesAsync();

                /*For Selecting all Possible Users and converting viewmodel 
                 * properties to entity model properties. 
                 * Does require the newly create project obj. Then commits*/
                if (selectedCollaborators != null)
                {

                    model.UserProjects = new List<UserProject>();
                    foreach (var user in selectedCollaborators)
                    {
                        try
                        {
                            var userid = (from a in _context.User
                                          where a.UserName == user
                                          select a).First<User>().UserID;
                            
                            UserProject userProjects = new UserProject
                            {
                                ProjectID = newProject.ProjectID,
                                UserID = userid
                            };

                            _context.Add(userProjects);
                        }
                        catch (InvalidOperationException e)
                        {
                            Console.WriteLine($"The user was not found: '{e}'");
                        }

                      
                    }
                   await _context.SaveChangesAsync();
                   return RedirectToAction(nameof(Index));
                    }
              PopulateAssignedProjectData(newProject);
            }
            return View();
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entityProjectModel = await _context.Projects
                .Include(i => i.UserProjects)
                .ThenInclude(i => i.User)
                .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ProjectID == id);

            if (entityProjectModel == null)
            {
                return NotFound();
            }
            PopulateAssignedProjectData(entityProjectModel);

            ProjectCreateViewModel viewModel = new ProjectCreateViewModel
            {
                Title = entityProjectModel.Title,
                Description = entityProjectModel.Description,
                StartDate = entityProjectModel.StartDate,
                EndDate = entityProjectModel.EndDate,
                CreationDate = entityProjectModel.CreationDate,
                PhotoPath = entityProjectModel.PhotoPath
            };
            return View(viewModel);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectID,Title,Description,StartDate,EndDate,CreationDate,PhotoPath,ProjectLinkOne,ProjectLinkTwo,ProjectLinkThree")] Project project)
        {
            if (id != project.ProjectID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectID))
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
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.ProjectID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectID == id);
        }
    }
}
