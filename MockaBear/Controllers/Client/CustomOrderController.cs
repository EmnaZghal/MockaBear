using Microsoft.AspNetCore.Mvc;

namespace MockaBear.Controllers.Client
{
    public class CustomOrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
