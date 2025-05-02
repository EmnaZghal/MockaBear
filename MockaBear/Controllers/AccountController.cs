using Microsoft.AspNetCore.Mvc;
using MockaBear.Data;
using MockaBear.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


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
                Password = HashPassword(Password),
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

        private string HashPassword(string password)
        {
            var passwordHasher = new PasswordHasher<object>();
            return passwordHasher.HashPassword(null, password);
        }

        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            var passwordHasher = new PasswordHasher<object>();

            // 1. Chercher dans Admin
            var admin = _context.Admins.FirstOrDefault(a => a.Email == Email);
            if (admin != null)
            {
                var adminPasswordVerification = passwordHasher.VerifyHashedPassword(null, admin.Password, Password);

                if (adminPasswordVerification == PasswordVerificationResult.Success)
                {
                    // Authentification admin réussie
                    HttpContext.Session.SetInt32("AdminId", admin.Id);
                    HttpContext.Session.SetString("AdminFirstName", admin.FirstName);
                    HttpContext.Session.SetString("AdminLastName", admin.LastName);

                    TempData["Success"] = "Admin logged in successfully!";
                    return RedirectToAction("Products", "Product");
                   
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    ViewBag.OpenLoginModal = true;
                    return View("~/Views/Home/Index.cshtml");
                }
            }

            // 2. Chercher dans Clients
            var client = _context.Clients.FirstOrDefault(c => c.Email == Email);
            if (client != null)
            {
                var clientPasswordVerification = passwordHasher.VerifyHashedPassword(null, client.Password, Password);

                if (clientPasswordVerification == PasswordVerificationResult.Success)
                {
                    // Authentification client réussie
                    HttpContext.Session.SetInt32("ClientId", client.Id);
                    HttpContext.Session.SetString("ClientFirstName", client.FirstName);
                    HttpContext.Session.SetString("ClientLastName", client.LastName);
                    HttpContext.Session.SetString("ClientImage", client.Image);
                    HttpContext.Session.SetString("ClientEmail", client.Email);
                    HttpContext.Session.SetString("ClientAdress", client.Adress);
                    HttpContext.Session.SetString("ClientPhone", client.Phone);
                    HttpContext.Session.SetString("ClientPassword", client.Password);

                    var cart = _context.Carts
                   .Include(c => c.Items)
                   .FirstOrDefault(c => !c.IsCheckedOut &&
                                        c.ClientId == client.Id);

                    if (cart == null)
                    {
                        cart = new Cart { ClientId = client.Id };
                        _context.Carts.Add(cart);
                        _context.SaveChanges();
                    }

                    // STOCKE L’ID DANS LA SESSION -----------------------
                    HttpContext.Session.SetInt32("CartId", cart.Id);

                    TempData["Success"] = "Logged in successfully!";
                    return RedirectToAction("Index", "Home");
                }
            }

            // 3. Aucun compte trouvé
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            ViewBag.OpenLoginModal = true;
            return View("~/Views/Home/Index.cshtml");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Remove("CartId");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }

}


