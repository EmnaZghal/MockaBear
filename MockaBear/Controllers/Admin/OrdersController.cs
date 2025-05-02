using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockaBear.Data;
using MockaBear.Models;

namespace MockaBear.Controllers.Admin
{
    [Route("admin/[controller]")]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;
        public OrdersController(AppDbContext context) => _context = context;

        [HttpGet("orders")]
        public IActionResult Orders(int page = 1, int pageSize = 10)
        {
            // 1) On prépare la query (ordre décroissant)
            var query = _context.Orders
                                .Include(o => o.Client)
                                .OrderByDescending(o => o.OrderDate);

            // 2) Nombre total de commandes
            var totalCount = query.Count();

            // 3) on ne prend que la page demandée
            var orders = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // 4) Passage des infos de pagination à la vue
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)System.Math.Ceiling((double)totalCount / pageSize);

            return View("~/Views/Admin/OrderViews/Orders.cshtml", orders);
        }
        // Retourne en JSON les items pour la modal
        [HttpGet("Details/{id}")]
        public IActionResult Details(int id)
        {
            var items = _context.OrderItems
                         .Include(oi => oi.Product)
                         .Where(oi => oi.OrderId == id)
                         .Select(oi => new {
                             productName = oi.Product.Name,
                             oi.Quantity,
                             unitPrice = oi.UnitPrice,
                             totale = oi.Totale
                         })
                         .ToList();
            return Json(new { items });
        }

        [HttpPost("UpdateStatus")]
        public IActionResult UpdateStatus(int id, string status)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return NotFound();
            order.Status = status;
            _context.SaveChanges();
            return RedirectToAction(nameof(Orders));
        }
    }
}
