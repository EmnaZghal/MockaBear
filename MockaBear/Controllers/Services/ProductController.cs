using Microsoft.AspNetCore.Mvc;

namespace MockaBear.Controllers.Services
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
