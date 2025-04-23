using Microsoft.AspNetCore.Mvc;

namespace MockaBear.Controllers.Services
{
    public class CustomOrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
