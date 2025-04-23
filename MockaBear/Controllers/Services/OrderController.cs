using Microsoft.AspNetCore.Mvc;

namespace MockaBear.Controllers.Services
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
