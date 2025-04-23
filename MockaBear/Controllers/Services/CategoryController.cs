using Microsoft.AspNetCore.Mvc;

namespace MockaBear.Controllers.Services
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
