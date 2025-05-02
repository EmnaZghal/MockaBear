using Microsoft.AspNetCore.Mvc;
using MockaBear.Data;
using MockaBear.Models;

namespace MockaBear.Controllers.Client
{
    [Route("reviews")]
    public class ProductReviewController : Controller
    {
        private readonly AppDbContext _context;

        public ProductReviewController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public IActionResult Create(int productId, int rating, string comment)
        {
            int? clientId = HttpContext.Session.GetInt32("ClientId");
            if (clientId == null)
                return RedirectToAction("Login", "Account");
            var review = new Review
            {
                ProductId = productId,
                ClientId = clientId.Value,
                Rating = rating,
                Comment = comment!.Trim(),   
                ReviewDate = DateTime.Now
            };

            _context.Reviews.Add(review);
            _context.SaveChanges();

            // 4) retour à la page produit
            return RedirectToAction(
                "Details",          // action
                "ProductList",      // **nom exact** de ton contrôleur
                new { id = productId });
        }

    }
}
