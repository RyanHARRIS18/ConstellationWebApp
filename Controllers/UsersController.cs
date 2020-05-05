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
    public class UsersController : Controller
    {
        private readonly ConstellationWebAppContext _context;
        private readonly IWebHostEnvironment hostingEnvironment;

        public UsersController(ConstellationWebAppContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;

        }
  
        private string ValidateImagePath(UserCreateViewModel model)
        {
            string uniqueFileName = null;
            if (!(System.IO.Path.GetExtension(model.Photo.FileName) == ".png" || System.IO.Path.GetExtension(model.Photo.FileName) == ".jpg"))
            {
                model.Photo = null;
            }
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath + "\\image\\");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
            }
            return(uniqueFileName);
        }

        private void CreateLinksValidation(string[] createdLinkLabels, string[] createdLinkUrls, User newUser)
        {
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
        }

        private UserEditViewModel userToEditViewModel(User entityProjectModel)
        {
            UserEditViewModel viewModel = new UserEditViewModel
            {
                UserID = entityProjectModel.UserID,
                UserName = entityProjectModel.UserName,
                Password = entityProjectModel.Password,
                FirstName = entityProjectModel.FirstName,
                LastName = entityProjectModel.LastName,
                Bio = entityProjectModel.Bio,
                Seeking = entityProjectModel.Seeking,
                OldPhotoPath = entityProjectModel.PhotoPath,
                PhotoPath = entityProjectModel.PhotoPath
            };
            return(viewModel);
        }

        private User ViewModeltoUser(UserCreateViewModel model, string uniqueFileName)
        {
            User newUser = new User
            {
                UserName = model.UserName,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Bio = model.Bio,
                Seeking = model.Seeking,
                PhotoPath = uniqueFileName
            };
            return (newUser);
        }

        private void RemoveAllContactLinks(int id)
        {
            var contactLinks = from m in _context.ContactLinks
                               select m;

            var linksId = contactLinks.Where(s => s.Users.UserID == id);

            foreach (var link in linksId)
            {
                _context.ContactLinks.Remove(link);
            }
        }

        private void DeletePhoto(UserEditViewModel model)
        {
            if (model.OldPhotoPath != null)
            {
                string filePath = Path.Combine(hostingEnvironment.WebRootPath, "image", model.OldPhotoPath);
                System.IO.File.Delete(filePath);
            }
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
             var uniqueFileName = ValidateImagePath(model);
             User newUser = ViewModeltoUser(model, uniqueFileName);
             CreateLinksValidation(createdLinkLabels, createdLinkUrls, newUser);
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
            var viewModel = userToEditViewModel(entityProjectModel);
            return View(viewModel);
        }


        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string OldPhotoPath, string[] createdLinkLabels, string[] createdLinkUrls, UserEditViewModel model)
        {
            var user = await _context.User.FindAsync(id);

            if (id != model.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                    string uniqueFileName = OldPhotoPath;
                    if (model.Photo != null)
                {
                    DeletePhoto(model);
                    uniqueFileName = ValidateImagePath(model);
                }
                user.UserName = model.UserName;
                user.Password = model.Password;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Bio = model.Bio;
                user.Seeking = model.Seeking;
                user.PhotoPath = uniqueFileName;
                        _context.Update(user);
                        await _context.SaveChangesAsync();
                        CreateLinksValidation(createdLinkLabels, createdLinkUrls, user);
                    
                    if (!UserExists(user.UserID))
                    {
                        return NotFound();
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
            RemoveAllContactLinks(id);

            var user = await _context.User.FindAsync(id);
            var viewModel = userToEditViewModel(user);
            DeletePhoto(viewModel);
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
