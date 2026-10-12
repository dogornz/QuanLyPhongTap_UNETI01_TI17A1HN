using Microsoft.AspNetCore.Mvc;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using System.Diagnostics;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
