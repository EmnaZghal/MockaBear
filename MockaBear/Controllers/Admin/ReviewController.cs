using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockaBear.Data;
using MockaBear.Models;

namespace MockaBear.Controllers.Admin
{
    [Route("admin/[controller]")]
    public class ReviewController : Controller
    {
        private readonly AppDbContext _context;

        public ReviewController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("reviews")]
        public IActionResult Reviews(int page = 1, int pageSize = 10)
        {
            var reviews = _context.Reviews
                .Include(r => r.Product)
                .Include(r => r.Client)
                .OrderByDescending(r => r.ReviewDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((double)_context.Reviews.Count() / pageSize);

            return View("~/Views/Admin/ReviewsViews/Reviews.cshtml", reviews);
        }

        [HttpPost("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var review = _context.Reviews.Find(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            _context.SaveChanges();

            return RedirectToAction("reviews");
        }
    }
}
