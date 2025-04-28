using Microsoft.AspNetCore.Mvc;
using MockaBear.Data;
using MockaBear.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MockaBear.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(string FirstName, string LastName, string Email, string Password, string Phone, string Adress, IFormFile ImageFile)
        {
            if (_context.Clients.Any(c => c.Email == Email))
            {
                ModelState.AddModelError("Email", "This email is already taken.");
                ViewBag.OpenSignUpModal = true; // pour rouvrir le modal
                return View("~/Views/Home/Index.cshtml");
            }

            string imageName = "default-profile.png";

            if (ImageFile != null && ImageFile.Length > 0)
            {
                imageName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/clients", imageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
            }

            var client = new MockaBear.Models.Client
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Password = Password,
                Phone = Phone,
                Adress = Adress,
                Image = imageName,
                DateAdded = DateTime.Now
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Account created successfully!";
            return RedirectToAction("Index", "Home");
        }

    }
}


