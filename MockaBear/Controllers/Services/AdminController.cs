using Microsoft.AspNetCore.Mvc;

namespace MockaBear.Controllers.Services
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
