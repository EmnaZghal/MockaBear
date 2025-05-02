using Microsoft.AspNetCore.Mvc;
using MockaBear.Data;
using MockaBear.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;


namespace MockaBear.Controllers.Admin
{
    [Route("admin/[controller]")]
    public class AdministratorController : Controller
    {
        private readonly AppDbContext _context;

        public AdministratorController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("administrators")]
        public IActionResult Administrators(int page = 1, int pageSize = 10)
        {
            var admins = _context.Admins
                                 .OrderBy(a => a.Id)
                                 .Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((double)_context.Admins.Count() / pageSize);

            
            return View("~/Views/Admin/AdminViews/administrators.cshtml", admins);
        }


        [HttpPost("add")]
        public async Task<IActionResult> Add(string firstName,
                                      string lastName,
                                      string email,
                                      string password)
        {
            if (_context.Admins.Any(a => a.Email == email))
            {
                ModelState.AddModelError("Email", "This e-mail is already in use.");
                ViewBag.OpenAddAdminModal = true;       


                var admins = _context.Admins.OrderBy(a => a.Id).ToList();
                return View("~/Views/Admin/AdminViews/administrators.cshtml", admins);
            }

            var admin = new MockaBear.Models.Admin
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = HashPassword(password)
            };
            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Administrators));
        }


        [HttpPost("update")]
        public async Task<IActionResult> Update(int id,
                                                string firstName,
                                                string lastName,
                                                string email,
                                                string? password)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null) return NotFound();

            admin.FirstName = firstName;
            admin.LastName = lastName;
            admin.Email = email;

            // Mettez à jour le mot de passe seulement si l’utilisateur en saisit un nouveau
            if (!string.IsNullOrWhiteSpace(password))
            {
                admin.Password = HashPassword(password);   
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Administrators));
        }



        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null) return NotFound();

            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Administrators));
        }



        private string HashPassword(string password)
        {
            var passwordHasher = new PasswordHasher<object>();
            return passwordHasher.HashPassword(null, password);
        }
    }
}
