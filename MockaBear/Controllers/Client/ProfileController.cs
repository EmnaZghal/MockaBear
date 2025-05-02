using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockaBear.Data;

namespace MockaBear.Controllers.Client
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;
        public ProfileController(AppDbContext context)
        {
            _context = context;
        }
    


   [HttpGet]
        public IActionResult ProfileDetails()
        {
            var clientId = HttpContext.Session.GetInt32("ClientId");
            if (clientId == null)
                return RedirectToAction("Index", "Home"); // ou login

            var client = _context.Clients
                                 .Include(c => c.Orders)
                                 .Include(c => c.Reviews)
                                 .FirstOrDefault(c => c.Id == clientId.Value);
            if (client == null)
                return NotFound();

            return View("~/Views/Home/ProfileDetails.cshtml", client);
        }
    }
}
