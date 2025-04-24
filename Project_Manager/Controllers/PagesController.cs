using Microsoft.AspNetCore.Mvc;

namespace Project_Manager.Controllers
{
    public class PagesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Booking()
        {
            return View();
        }
        public IActionResult OurTeam()
        {
            return View();
        }
        public IActionResult Testimonial()
        {
            return View();
        }


    }
}
