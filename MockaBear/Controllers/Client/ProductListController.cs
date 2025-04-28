using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockaBear.Data;
using MockaBear.Models;

namespace MockaBear.Controllers.Client
{
    [Route("espaceClient/[controller]")]
    public class ProductListController : Controller
    {
        private readonly AppDbContext _context;


        public ProductListController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("products")]
        public IActionResult Products(int page = 1, int pageSize = 10)
        {
            var products = _context.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.DateAdded)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((double)_context.Products.Count() / pageSize);

            ViewBag.Categories = _context.Categories.ToList();

            return View("~/Views/Home/ProductList.cshtml", products);
        }
    }
}
