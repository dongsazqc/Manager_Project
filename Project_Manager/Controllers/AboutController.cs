using Microsoft.AspNetCore.Mvc;

namespace Project_Manager.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
