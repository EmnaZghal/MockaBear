using Microsoft.AspNetCore.Mvc;

namespace MockaBear.Controllers.Admin
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
