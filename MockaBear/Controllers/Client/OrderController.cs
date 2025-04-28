using Microsoft.AspNetCore.Mvc;

namespace MockaBear.Controllers.Client
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
