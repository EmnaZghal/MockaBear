using Microsoft.AspNetCore.Mvc;

namespace MockaBear.Controllers.Services
{
    public class ReviewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
