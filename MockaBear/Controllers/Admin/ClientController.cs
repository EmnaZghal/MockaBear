using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockaBear.Data;

namespace MockaBear.Controllers.Admin
{
    [Route("admin/[controller]")]
    public class ClientController : Controller
    {
        private readonly AppDbContext _context;
        public ClientController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("Clients")]
        public IActionResult Clients(int page = 1, int pageSize = 10)
        {
            var clients = _context.Clients
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((double)_context.Clients.Count() / pageSize);

            return View("~/Views/Admin/ClientsViews/Clients.cshtml", clients);
        }

        [HttpPost("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var client = _context.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            _context.SaveChanges();

            return RedirectToAction("Clients");
        }
    }
}
