using Microsoft.AspNetCore.Mvc;

namespace MockaBear.Controllers.Client
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProductList()
        {
            // Tu peux passer une liste de produits ici via le modèle
            return View();
        }
        [HttpGet]
        public IActionResult AboutUs()
        {
            return View(); // cherchera Views/Home/AboutUs.cshtml
        }
    }
}
