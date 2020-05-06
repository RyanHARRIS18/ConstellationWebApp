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
using ConstellationWebApp.Models.ViewModels;

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

        private void CreateProjectLinks(string[] createdLinkLabels, string[] createdLinkUrls, Project newProject)
        {
            if (createdLinkLabels != null)
            {
                for (var i = 0; i < createdLinkLabels.Length; i++)
                {
                    ProjectLink newContact = new ProjectLink
                    {
                        ProjectLinkLabel = createdLinkLabels[i],
                        ProjectLinkUrl = createdLinkUrls[i],
                        Projects = newProject
                    };
                    _context.Add(newContact);
                }
            }
        }

        private string ValidateImagePath(ProjectCreateViewModel model)
        {
            string uniqueFileName = null;
            if (!(System.IO.Path.GetExtension(model.Photo.FileName) == ".png" || System.IO.Path.GetExtension(model.Photo.FileName) == ".jpg"))
            {
                model.Photo = null;
            }
            if (model.Photo != null)
            {
                uniqueFileName = System.IO.Path.GetFileName(model.Photo.FileName);
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath + "\\image\\");
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
            }
            return (uniqueFileName);
        }

        // Get Projects
        public async Task<IActionResult> Index()
        {
            var viewModel = new ViewModel();
            viewModel.Projects = await _context.Projects
                   .Include(i => i.UserProjects)
                     .ThenInclude(i => i.User)
                      .Include(i => i.ProjectLinks)
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

        private Project projectViewModelToUser(ProjectCreateViewModel model, string uniqueFileName)
        {
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
            return newProject;
        }

        private static ProjectEditViewModel projectToViewModel(Project entityProjectModel)
        {
            return new ProjectEditViewModel
            {
                ProjectID = entityProjectModel.ProjectID,
                Title = entityProjectModel.Title,
                Description = entityProjectModel.Description,
                StartDate = entityProjectModel.StartDate,
                EndDate = entityProjectModel.EndDate,
                CreationDate = entityProjectModel.CreationDate,
                OldPhotoPath = entityProjectModel.PhotoPath
            };
        }

        private void DeletePhoto(ProjectEditViewModel model)
        {
            if (model.OldPhotoPath != null)
            {
                string filePath = Path.Combine(hostingEnvironment.WebRootPath, "image", model.OldPhotoPath);
                System.IO.File.Delete(filePath);
            }
        }

        private void RemoveAllProjectLinks(int id)
        {
            var projectLinks = from m in _context.ProjectLinks
                               select m;

            var linksId = projectLinks.Where(s => s.Projects.ProjectID == id);

            foreach (var link in linksId)
            {
                _context.ProjectLinks.Remove(link);
            }
        }

        // GET: Projects/Create
        public IActionResult Create(string SearchString)
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
        public async Task<IActionResult> Create(ProjectCreateViewModel model, string[] selectedCollaborators, string[] createdLinkLabels, string[] createdLinkUrls)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ValidateImagePath(model);    
                Project newProject = projectViewModelToUser(model, uniqueFileName);
                await _context.SaveChangesAsync();
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

                    CreateProjectLinks(createdLinkLabels, createdLinkUrls, newProject);

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
                .Include(i => i.ProjectLinks)
                .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ProjectID == id);
            if (entityProjectModel == null)
            {
                return NotFound();
            }
            PopulateAssignedProjectData(entityProjectModel);
            ProjectCreateViewModel viewModel = projectToViewModel(entityProjectModel);
            return View(viewModel);
        }   

    
        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectEditViewModel model, string[] selectedCollaborators, string[] createdLinkLabels, string[] createdLinkUrls, string OldPhotoPath)
        {
            var project = await _context.Projects.FindAsync(id);

            if (id != model.ProjectID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string uniqueFileName = OldPhotoPath;
                if (model.Photo != null)
                {
                    uniqueFileName = ValidateImagePath(model);
                    DeletePhoto(model);
                }
                project.Title = model.Title;
                project.Description = model.Description;
                project.StartDate = model.StartDate;
                project.EndDate = model.EndDate;
                project.CreationDate = model.CreationDate;
                project.PhotoPath = uniqueFileName;
                _context.Update(project);
                await _context.SaveChangesAsync();

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
                                ProjectID = project.ProjectID,
                                UserID = userid
                            };

                            _context.Add(userProjects);
                        }
                        catch (InvalidOperationException e)
                        {
                            Console.WriteLine($"The user was not found: '{e}'");
                        }
                    }

                    CreateProjectLinks(createdLinkLabels, createdLinkUrls, project);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                PopulateAssignedProjectData(project);
            }
            return View();
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entityProjectModel = await _context.Projects
                .Include(i => i.UserProjects)
                .ThenInclude(i => i.User)
                .Include(i => i.ProjectLinks)
                .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ProjectID == id);
            if (entityProjectModel == null)
            {
                return NotFound();
            }
            PopulateAssignedProjectData(entityProjectModel);
            ProjectCreateViewModel viewModel = projectToViewModel(entityProjectModel);
            return View(viewModel);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            RemoveAllProjectLinks(id);
            var project = await _context.Projects.FindAsync(id);
            var viewModel = projectToViewModel(project);
            DeletePhoto(viewModel);
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
