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

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
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
        public async Task<IActionResult> Create(UserCreateViewModel model, string[] createdLinkLabels, string[] createdLinkUls)
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
                    var linkData = createdLinkLabels.Join(createdLinkUls, label => label, url => url, (label, url) => new { labelCol = label, urlCol = url });

                    foreach (var link in linkData)
                    {

                        try
                        {
                            ContactLink contactLink = new ContactLink
                            {
                                ContactLinkLabel = link.labelCol,
                                ContactLinkUrl = link.urlCol
                            };

                            _context.Add(contactLink);
                            await _context.SaveChangesAsync();
                        }
                        catch (InvalidOperationException e)
                        {
                            Console.WriteLine($"The link or label was not found or was not in valid format: '{e}'");
                        }

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

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
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
