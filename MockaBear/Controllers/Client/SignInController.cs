using Microsoft.AspNetCore.Mvc;

namespace MockaBear.Controllers.Client
{
    public class SignInController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
