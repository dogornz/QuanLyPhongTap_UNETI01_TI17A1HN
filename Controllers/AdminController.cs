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
        public async Task<IActionResult> Index()
        {
            // 1. Thống kê tài khoản
            ViewBag.TotalAccounts = await _context.TaiKhoan.CountAsync();
            ViewBag.ActiveAccounts = await _context.TaiKhoan.CountAsync(t => t.TrangThai == "Unlock");
            ViewBag.AdminAccounts = await _context.TaiKhoan.CountAsync(t => t.VaiTro == "admin");
            ViewBag.UserAccounts = await _context.TaiKhoan.CountAsync(t => t.VaiTro != "admin");

            // 2. Thống kê hội viên & gói tập
            ViewBag.TotalMembers = await _context.HoiVien.CountAsync();
            ViewBag.TotalCategories = await _context.LoaiGoiTap.CountAsync();
            ViewBag.TotalPackages = await _context.GoiTap.CountAsync();
            ViewBag.ActivePackages = await _context.GoiTap.CountAsync(g => g.TrangThai == "Đang áp dụng");

            // 3. Thống kê phiếu đăng ký
            ViewBag.TotalRegistrations = await _context.PhieuDangKy.CountAsync();
            ViewBag.ActiveRegistrations = await _context.PhieuDangKy.CountAsync(p => p.TrangThai == "Đang hiệu lực" || p.TrangThai == "Đã xác nhận");
            ViewBag.PendingRegistrations = await _context.PhieuDangKy.CountAsync(p => p.TrangThai == "Chờ xác nhận");
            ViewBag.CancelledRegistrations = await _context.PhieuDangKy.CountAsync(p => p.TrangThai == "Đã hủy");

            // 4. Doanh thu
            ViewBag.TotalRevenue = await _context.PhieuDangKy
                .Where(p => p.TrangThai == "Đang hiệu lực" || p.TrangThai == "Đã xác nhận")
                .SumAsync(p => (decimal?)p.TongTien) ?? 0m;

            // 5. Phiếu đăng ký chờ duyệt gần nhất
            ViewBag.PendingTickets = await _context.PhieuDangKy
                .Include(p => p.HoiVien)
                .Include(p => p.GoiTap)
                .Where(p => p.TrangThai == "Chờ xác nhận")
                .OrderByDescending(p => p.NgayDangKy)
                .Take(5)
                .ToListAsync();

            // 6. Gói tập được đăng ký nhiều nhất
            var topGrouped = await _context.PhieuDangKy
                .GroupBy(p => p.MaGoiTap)
                .Select(g => new { MaGoiTap = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();

            var topPackagesList = new List<dynamic>();
            foreach (var item in topGrouped)
            {
                var pkg = await _context.GoiTap.FindAsync(item.MaGoiTap);
                if (pkg != null)
                {
                    topPackagesList.Add(new { Package = pkg, Count = item.Count });
                }
            }
            ViewBag.TopPackages = topPackagesList;

            return View();
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