using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using QuanLyPhongTap_UNETI01_TI17A1HN.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Controllers
{
    [Authorize(Roles = "admin,Admin")]
    public class AdminController : Controller
    {
        private readonly QuanLyPhongTap_UNETI01_TI17A1HNContext _context;
        private readonly DashboardService _dashboardService;
        private readonly ThongKeService _thongKeService; // THÊM

        public AdminController(
            QuanLyPhongTap_UNETI01_TI17A1HNContext context,
            DashboardService dashboardService,
            ThongKeService thongKeService) // THÊM
        {
            _context = context;
            _dashboardService = dashboardService;
            _thongKeService = thongKeService; // THÊM
        }

        // GET: /Admin or /Admin/Index
        public IActionResult Index()
        {
            return RedirectToAction("Index", "TaiKhoans");
        }

        // GET: /Admin/Dashboard
        public IActionResult Dashboard()
        {
            var vm = _dashboardService.Get();
            return View(vm);
        }

        // GET: /Admin/ThongKe
        public IActionResult ThongKe() // THÊM
        {
            var vm = _thongKeService.Get();
            return View(vm);
        }
    }
}