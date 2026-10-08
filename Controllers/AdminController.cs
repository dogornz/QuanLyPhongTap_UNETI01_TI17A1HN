using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly QuanLyPhongTap_UNETI01_TI17A1HNContext _context;

        public AdminController(QuanLyPhongTap_UNETI01_TI17A1HNContext context)
        {
            _context = context;
        }

        // GET: /Admin or /Admin/Index
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalAccounts = await _context.TaiKhoan.CountAsync();
            ViewBag.ActiveAccounts = await _context.TaiKhoan.CountAsync(t => t.TrangThai == "Unlock");
            ViewBag.AdminAccounts = await _context.TaiKhoan.CountAsync(t => t.VaiTro == "admin");
            ViewBag.UserAccounts = await _context.TaiKhoan.CountAsync(t => t.VaiTro != "admin");

            return View();
        }
    }
}
