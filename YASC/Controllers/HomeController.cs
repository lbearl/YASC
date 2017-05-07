using Microsoft.AspNetCore.Mvc;

namespace YASC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult About()
        {
            return View();
        }
    }
}
