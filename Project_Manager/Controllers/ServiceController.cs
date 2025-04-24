using Microsoft.AspNetCore.Mvc;

namespace Project_Manager.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
