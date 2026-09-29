using Gym_Management_Webapp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gym_Management_Webapp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly Gym_Management_WebappContext _context;

        public AdminController(Gym_Management_WebappContext context)
        {
            _context = context;
        }

        // GET: /Admin or /Admin/Index
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalAccounts = await _context.TaiKhoan.CountAsync();
            ViewBag.ActiveAccounts = await _context.TaiKhoan.CountAsync(t => t.TrangThai == "Active");
            ViewBag.AdminAccounts = await _context.TaiKhoan.CountAsync(t => t.VaiTro == "Admin");
            ViewBag.UserAccounts = await _context.TaiKhoan.CountAsync(t => t.VaiTro != "Admin");

            return View();
        }
    }
}
