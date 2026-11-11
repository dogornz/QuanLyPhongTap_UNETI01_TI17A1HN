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
        private readonly ThongKeService _thongKeService;

        public AdminController(
            QuanLyPhongTap_UNETI01_TI17A1HNContext context,
            DashboardService dashboardService,
            ThongKeService thongKeService)
        {
            _context = context;
            _dashboardService = dashboardService;
            _thongKeService = thongKeService;
        }

        // GET: /Admin or /Admin/Index
        public async Task<IActionResult> Index()
        {
            // 1. Tài khoản - Module 1
            ViewBag.TotalAccounts = await _context.TaiKhoan.CountAsync();
            ViewBag.ActiveAccounts = await _context.TaiKhoan.CountAsync(t => t.TrangThai == "Unlock");
            ViewBag.AdminAccounts = await _context.TaiKhoan.CountAsync(t => t.VaiTro == "admin");
            ViewBag.UserAccounts = await _context.TaiKhoan.CountAsync(t => t.VaiTro != "admin");

            // 2. Hội viên & gói tập - lấy từ DashboardService (Module 4), không tự đếm lại
            var dashboard = _dashboardService.Get();
            ViewBag.TotalMembers = dashboard.TongHoiVien;
            ViewBag.TotalCategories = dashboard.TongLoaiGoi;
            ViewBag.TotalPackages = dashboard.TongGoiTap;
            ViewBag.ActivePackages = dashboard.GoiDangApDung;

            // 3. Phiếu đăng ký - lấy từ DashboardService + ThongKeService
            ViewBag.TotalRegistrations = dashboard.TongPhieu;
            ViewBag.ActiveRegistrations = dashboard.DangHieuLuc;
            ViewBag.PendingRegistrations = dashboard.PhieuChoXacNhan;

            var phieuTheoTrangThai = _thongKeService.PhieuTheoTrangThai();
            ViewBag.CancelledRegistrations = phieuTheoTrangThai
                .FirstOrDefault(x => x.Ten == TrangThaiPhieu.DaHuy)?.SoLuong ?? 0;

            // 4. Doanh thu - lấy từ ThongKeService
            ViewBag.TotalRevenue = _thongKeService.DoanhThuTheoThang().Sum(x => x.DoanhThu);

            // 5. Phiếu đăng ký chờ duyệt gần nhất
            ViewBag.PendingTickets = await _context.PhieuDangKy
                .Include(p => p.HoiVien)
                .Include(p => p.GoiTap)
                .Where(p => p.TrangThai == TrangThaiPhieu.ChoXacNhan)
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