using Microsoft.AspNetCore.Mvc;
using MockaBear.Data;
using MockaBear.Models;
using System.Linq;

namespace MockaBear.Controllers.Admin
{
    [Route("admin/[controller]")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("categories")]
        public IActionResult Categories(int page = 1, int pageSize = 10)
        {
            var categories = _context.Categories
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((double)_context.Categories.Count() / pageSize);

            return View("~/Views/Admin/CategoriesViews/Categories.cshtml", categories);
        }

        [HttpPost("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction("Categories");
        }


        [HttpPost("add")]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Category category)
        {
            Console.WriteLine(">>> Add method triggered");
            Console.WriteLine("Name: " + category.Name);
            Console.WriteLine("Description: " + category.Description);

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("Error: " + error.ErrorMessage);
                }

                return RedirectToAction("Categories");
            }

            _context.Categories.Add(category);
            _context.SaveChanges();
            Console.WriteLine("Saved!");

            return RedirectToAction("Categories");
        }

        [HttpPost("update")]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Category updatedCategory)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == updatedCategory.Id);
            if (category == null)
                return NotFound();

            category.Name = updatedCategory.Name;
            category.Description = updatedCategory.Description;

            _context.SaveChanges();
            return RedirectToAction("Categories");
        }


    }
}