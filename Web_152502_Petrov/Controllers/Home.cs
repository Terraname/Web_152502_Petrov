using Microsoft.AspNetCore.Mvc;

namespace Web_152502_Petrov.Controllers
{
    public class Home : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        //public IActionResult Privacy()
        //{
        //    return View();
        //}
    }
}
