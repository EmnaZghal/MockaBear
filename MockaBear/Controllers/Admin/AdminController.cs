using Microsoft.AspNetCore.Mvc;

namespace MockaBear.Controllers.Admin
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard(string section = "products")
        {
            ViewBag.Section = section;
            return View();
        }
    }
}
