using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using QuanLyPhongTap_UNETI01_TI17A1HN.Services;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Controllers
{
    [Authorize]
    public class PhieuDangKysController : Controller
    {
        private readonly PhieuDangKyService _service;

        public PhieuDangKysController(PhieuDangKyService service)
        {
            _service = service;
        }

        // Helper tìm Hội viên của tài khoản đăng nhập hiện tại
        private HoiVien? GetCurrentHoiVien()
        {
            var maTaiKhoan = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.FindFirst("Username")?.Value ?? User.Identity?.Name;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var name = User.Identity?.Name;

            var allHoiViens = _service.GetHoiViens();
            return allHoiViens.FirstOrDefault(h =>
                (!string.IsNullOrEmpty(maTaiKhoan) && h.MaTaiKhoan == maTaiKhoan)
                || (!string.IsNullOrEmpty(username) && (h.MaHoiVien.Equals(username, StringComparison.OrdinalIgnoreCase) || (!string.IsNullOrEmpty(h.Email) && h.Email.Equals(username, StringComparison.OrdinalIgnoreCase))))
                || (!string.IsNullOrEmpty(email) && (!string.IsNullOrEmpty(h.Email) && h.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
                || (!string.IsNullOrEmpty(name) && (h.HoTen.Equals(name, StringComparison.OrdinalIgnoreCase) || h.MaHoiVien.Equals(name, StringComparison.OrdinalIgnoreCase)))
            );
        }

        [HttpGet]
        public IActionResult Index(string? maHoiVien, string? maGoiTap, string? trangThai, DateTime? tuNgay, DateTime? denNgay)
        {
            bool isAdmin = User.IsInRole("Admin") || User.IsInRole("admin");
            HoiVien? currentHoiVien = null;

            if (!isAdmin)
            {
                currentHoiVien = GetCurrentHoiVien();
                if (currentHoiVien != null)
                {
                    // Hội viên chỉ được xem phiếu của chính mình
                    maHoiVien = currentHoiVien.MaHoiVien;
                }
                else
                {
                    // Nếu tài khoản không thuộc hội viên nào, trả về danh sách rỗng để bảo mật
                    ViewBag.IsAdmin = false;
                    ViewBag.CurrentHoiVien = null;
                    return View(new List<PhieuDangKy>());
                }
            }

            var danhSach = _service.TimKiem(maHoiVien, maGoiTap, trangThai, tuNgay, denNgay);

            ViewBag.IsAdmin = isAdmin;
            ViewBag.CurrentHoiVien = currentHoiVien;
            ViewBag.MaHoiVien = maHoiVien;
            ViewBag.MaGoiTap = maGoiTap;
            ViewBag.TrangThai = trangThai;
            ViewBag.TuNgay = tuNgay?.ToString("yyyy-MM-dd");
            ViewBag.DenNgay = denNgay?.ToString("yyyy-MM-dd");

            ViewBag.GoiTaps = new SelectList(
                _service.GetGoiTaps(),
                "MaGoiTap",
                "TenGoiTap",
                maGoiTap);

            if (isAdmin)
            {
                ViewBag.HoiViens = new SelectList(
                    _service.GetHoiViens(),
                    "MaHoiVien",
                    "HoTen",
                    maHoiVien);
            }
            else if (currentHoiVien != null)
            {
                ViewBag.HoiViens = new SelectList(
                    new List<HoiVien> { currentHoiVien },
                    "MaHoiVien",
                    "HoTen",
                    currentHoiVien.MaHoiVien);
            }

            return View(danhSach);
        }

        [HttpGet]
        public IActionResult Create()
        {
            bool isAdmin = User.IsInRole("Admin") || User.IsInRole("admin");
            HoiVien? currentHoiVien = !isAdmin ? GetCurrentHoiVien() : null;

            ViewBag.IsAdmin = isAdmin;
            ViewBag.CurrentHoiVien = currentHoiVien;

            NapDanhSach(currentHoiVien?.MaHoiVien, null, currentHoiVien);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
            string maHoiVien,
            string maGoiTap,
            DateTime ngayBatDau,
            bool xacNhanGoiKhac = false)
        {
            bool isAdmin = User.IsInRole("Admin") || User.IsInRole("admin");
            HoiVien? currentHoiVien = !isAdmin ? GetCurrentHoiVien() : null;

            // Nếu là Hội viên: Ép buộc đăng ký cho chính mình, cấm đăng ký cho người khác
            if (!isAdmin)
            {
                if (currentHoiVien == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin hội viên của tài khoản đang đăng nhập!";
                    return RedirectToAction(nameof(Index));
                }

                // Bảo mật: ép mã hội viên về chính mình
                maHoiVien = currentHoiVien.MaHoiVien;
            }

            ViewBag.IsAdmin = isAdmin;
            ViewBag.CurrentHoiVien = currentHoiVien;

            var ketQua = _service.DangKy(
                maHoiVien,
                maGoiTap,
                ngayBatDau,
                xacNhanGoiKhac);

            if (ketQua.Success)
            {
                TempData["Success"] = ketQua.Message;
                return RedirectToAction(nameof(Index));
            }

            if (ketQua.RequireConfirm)
            {
                ViewBag.RequireConfirm = true;
                ViewBag.ConfirmMessage = ketQua.Message;
                ViewBag.TenGoiHienTai = ketQua.TenGoiHienTai;
                ViewBag.SelectedMaHoiVien = maHoiVien;
                ViewBag.SelectedMaGoiTap = maGoiTap;
                ViewBag.SelectedNgayBatDau = ngayBatDau.ToString("yyyy-MM-dd");
                NapDanhSach(maHoiVien, maGoiTap, currentHoiVien);
                return View();
            }

            TempData["Error"] = ketQua.Message;
            NapDanhSach(maHoiVien, maGoiTap, currentHoiVien);
            return View();
        }

        // API kiểm tra tình trạng gói tập của hội viên (cho Ajax gọi từ Create.cshtml)
        [HttpGet]
        public IActionResult KiemTraGoiTap(string maHoiVien, string? maGoiTap)
        {
            bool isAdmin = User.IsInRole("Admin") || User.IsInRole("admin");
            if (!isAdmin)
            {
                var currentHoiVien = GetCurrentHoiVien();
                if (currentHoiVien != null)
                {
                    maHoiVien = currentHoiVien.MaHoiVien;
                }
            }

            if (string.IsNullOrWhiteSpace(maHoiVien))
            {
                return Json(new { hasActive = false });
            }

            var activeTicket = _service.GetActiveTicket(maHoiVien);
            if (activeTicket == null)
            {
                return Json(new { hasActive = false });
            }

            string tenGoiHienTai = activeTicket.GoiTap?.TenGoiTap ?? activeTicket.MaGoiTap;
            bool isSamePackage = (!string.IsNullOrWhiteSpace(maGoiTap) && activeTicket.MaGoiTap == maGoiTap);

            return Json(new
            {
                hasActive = true,
                isSamePackage = isSamePackage,
                maGoiHienTai = activeTicket.MaGoiTap,
                tenGoiHienTai = tenGoiHienTai,
                ngayBatDauHienTai = activeTicket.NgayBatDau.ToString("dd/MM/yyyy"),
                ngayKetThucHienTai = activeTicket.NgayKetThuc.ToString("dd/MM/yyyy"),
                ngayKetThucHienTaiIso = activeTicket.NgayKetThuc.ToString("yyyy-MM-dd"),
                canhBao = isSamePackage
                    ? $"Hội viên đang trong thời gian sử dụng gói tập {tenGoiHienTai} (Thời gian hoạt động: từ {activeTicket.NgayBatDau:dd/MM/yyyy} đến {activeTicket.NgayKetThuc:dd/MM/yyyy}). Hệ thống không cho phép đăng ký thêm gói này, chỉ có quyền gia hạn thêm!"
                    : $"Bạn đang trong thời gian sử dụng gói tập {tenGoiHienTai} (Thời gian hoạt động: từ {activeTicket.NgayBatDau:dd/MM/yyyy} đến {activeTicket.NgayKetThuc:dd/MM/yyyy}). Vui lòng xem kỹ thời gian bắt đầu của gói mới! Bạn có xác nhận đăng ký gói khác không?"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XacNhan(string id)
        {
            if (!User.IsInRole("Admin") && !User.IsInRole("admin"))
            {
                TempData["Error"] = "Hội viên không có quyền duyệt phiếu đăng ký! Thao tác này chỉ dành cho Quản trị viên.";
                return RedirectToAction(nameof(Index));
            }

            var res = _service.XacNhan(id);
            if (res.Success)
            {
                TempData["Success"] = res.Message;
            }
            else
            {
                TempData["Error"] = res.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Huy(string id, string? returnUrl = null)
        {
            bool isAdmin = User.IsInRole("Admin") || User.IsInRole("admin");
            if (!isAdmin)
            {
                var currentHoiVien = GetCurrentHoiVien();
                var phieu = _service.GetById(id);
                if (phieu == null || currentHoiVien == null || phieu.MaHoiVien != currentHoiVien.MaHoiVien)
                {
                    TempData["Error"] = "Bạn không có quyền thao tác vào phiếu của hội viên khác!";
                    return RedirectAfterAction(returnUrl);
                }
            }

            var res = _service.Huy(id);
            if (res.Success)
            {
                TempData["Success"] = res.Message;
            }
            else
            {
                TempData["Error"] = res.Message;
            }
            return RedirectAfterAction(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GiaHan(string id, int soThang = 1, string? returnUrl = null)
        {
            bool isAdmin = User.IsInRole("Admin") || User.IsInRole("admin");
            if (!isAdmin)
            {
                var currentHoiVien = GetCurrentHoiVien();
                var phieu = _service.GetById(id);
                if (phieu == null || currentHoiVien == null || phieu.MaHoiVien != currentHoiVien.MaHoiVien)
                {
                    TempData["Error"] = "Bạn không có quyền thao tác vào phiếu của hội viên khác!";
                    return RedirectAfterAction(returnUrl);
                }
            }

            var res = _service.GiaHan(id, soThang, isHoiVien: !isAdmin);
            if (res.Success)
            {
                TempData["Success"] = res.Message;
            }
            else
            {
                TempData["Error"] = res.Message;
            }
            return RedirectAfterAction(returnUrl);
        }

        private IActionResult RedirectAfterAction(string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }
            return RedirectToAction(nameof(Index));
        }

        private void NapDanhSach(
            string? maHoiVien = null,
            string? maGoiTap = null,
            HoiVien? currentHoiVien = null)
        {
            bool isAdmin = User.IsInRole("Admin") || User.IsInRole("admin");

            if (!isAdmin && currentHoiVien != null)
            {
                ViewBag.HoiViens = new SelectList(
                    new List<HoiVien> { currentHoiVien },
                    "MaHoiVien",
                    "HoTen",
                    currentHoiVien.MaHoiVien);
            }
            else
            {
                ViewBag.HoiViens = new SelectList(
                    _service.GetHoiViens(),
                    "MaHoiVien",
                    "HoTen",
                    maHoiVien);
            }

            ViewBag.GoiTaps = new SelectList(
                _service.GetGoiTaps(),
                "MaGoiTap",
                "TenGoiTap",
                maGoiTap);
        }
    }
}
