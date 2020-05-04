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
    public class UsersController : Controller
    {
        private readonly ConstellationWebAppContext _context;
        private readonly IWebHostEnvironment hostingEnvironment;


        public UsersController(ConstellationWebAppContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;

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

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var viewModel = new ViewModel();
            viewModel.Users = await _context.User
                  .Include(i => i.ContactLinks)
                   .AsNoTracking()
                   .OrderBy(i => i.UserID)
                   .ToListAsync();
            return View(viewModel);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel model, string[] createdLinkLabels, string[] createdLinkUrls)
        {
            if (ModelState.IsValid)
            {
                    string uniqueFileName = null;

                    if (model.Photo != null)
                    {
                        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath + "\\image\\");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                    }

                    User newUser = new User
                    {             
                        UserID           = model.UserID,
                        UserName         = model.UserName,
                        Password         = model.Password,
                        FirstName        = model.FirstName,
                        LastName         = model.LastName,
                        Bio              = model.Bio,
                        Seeking          = model.Seeking,
                        PhotoPath        = uniqueFileName
                    };

                if (createdLinkLabels != null)
                {
                    for (var i = 0; i < createdLinkLabels.Length; i++)
                    {
                        ContactLink newContact = new ContactLink
                        {
                            ContactLinkLabel = createdLinkLabels[i],
                            ContactLinkUrl = createdLinkUrls[i],
                            Users = newUser
                        };

                    _context.Add(newContact);

                }
            }
                _context.Add(newUser);
                    await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entityProjectModel = await _context.User
                .Include(i => i.UserProjects)
                .ThenInclude(i => i.Project)
                .Include(i => i.ContactLinks)
                .AsNoTracking()
            .FirstOrDefaultAsync(m => m.UserID == id);

            if (entityProjectModel == null)
            {
                return NotFound();
            }
            UserCreateViewModel viewModel = new UserCreateViewModel
            {
                UserID = entityProjectModel.UserID,
                UserName = entityProjectModel.UserName,
                Password = entityProjectModel.Password,
                FirstName = entityProjectModel.FirstName,
                LastName = entityProjectModel.LastName,
                Bio = entityProjectModel.Bio,
                Seeking = entityProjectModel.Seeking,
                PhotoPath = entityProjectModel.PhotoPath
            };
            return View(viewModel);
        }


        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,UserName,Password,FirstName,LastName,Bio,Seeking,PhotoPath,ContactLinkOne,ContactLinkTwo,ContactLinkThree")] User user)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var contactLinks = from m in _context.ContactLinks
                               select m;

            var linksId = contactLinks.Where(s => s.Users.UserID == id);

            foreach (var link in linksId)
            {
                _context.ContactLinks.Remove(link);
            }

            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserID == id);
        }
    }
}
