using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockaBear.Data;
using MockaBear.Models;


namespace MockaBear.Controllers.Admin
{
    [Route("admin/[controller]")]
    public class IngredientController : Controller

    {

        private readonly AppDbContext _context;

        public IngredientController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("ingredients")]
        public IActionResult Ingredients(int page = 1, int pageSize = 10)
        {
            var ingredients = _context.Ingredients
                .OrderBy(i => i.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((double)_context.Ingredients.Count() / pageSize);

            return View("~/Views/Admin/IngredientViews/Ingredients.cshtml", ingredients);
        }
        [HttpPost("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var ingredient = _context.Ingredients.Find(id);
            if (ingredient == null)
            {
                return NotFound();
            }

            // Supprimer l'image du disque
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/ingredients", ingredient.ImageUrl);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            // Supprimer l'ingrédient de la base
            _context.Ingredients.Remove(ingredient);
            _context.SaveChanges();

            return RedirectToAction("Ingredients");
        }


        [HttpPost("add")]
        public async Task<IActionResult> Add(IFormFile ImageFile, string Name, decimal Price, int Number, string Task, int TaskNumber)
        {
            if (ImageFile == null || ImageFile.Length == 0)
                return Content("No file selected");

            var imageName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
            var imagePath = Path.Combine("wwwroot/images/ingredients", imageName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            var ingredient = new Ingredient
            {
                Name = Name,
                Price = Price,
                Number = Number,
                Task = Task,
                TaskNumber = TaskNumber,
                ImageUrl = imageName
            };

            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();

            return RedirectToAction("Ingredients");
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(int Id, string Name, decimal Price, int Number, string Task, int TaskNumber, string OldImageUrl, IFormFile? ImageFile)
        {
            var ingredient = await _context.Ingredients.FindAsync(Id);
            if (ingredient == null) return NotFound();

            ingredient.Name = Name;
            ingredient.Price = Price;
            ingredient.Number = Number;
            ingredient.Task = Task;
            ingredient.TaskNumber = TaskNumber;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Supprimer l’ancienne image
                var oldPath = Path.Combine("wwwroot/images/ingredients", OldImageUrl);
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);

                // Sauvegarder la nouvelle
                var newImageName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var newPath = Path.Combine("wwwroot/images/ingredients", newImageName);
                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                ingredient.ImageUrl = newImageName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Ingredients");
        }

    }
}
