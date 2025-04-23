using Microsoft.AspNetCore.Mvc;

namespace MockaBear.Controllers.Services
{
    public class IngredientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
