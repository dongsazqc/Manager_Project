using Microsoft.AspNetCore.Mvc;

namespace Project_Manager.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
