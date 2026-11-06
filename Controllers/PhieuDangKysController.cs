using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [HttpGet]
        public IActionResult Index(string? maHoiVien)
        {
            var danhSach = string.IsNullOrWhiteSpace(maHoiVien)
                ? _service.GetAll()
                : _service.GetByHoiVien(maHoiVien);

            ViewBag.MaHoiVien = maHoiVien;

            ViewBag.HoiViens = new SelectList(
                _service.GetHoiViens(),
                "MaHoiVien",
                "HoTen",
                maHoiVien);

            return View(danhSach);
        }

        [HttpGet]
        public IActionResult Create()
        {
            NapDanhSach();
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
                NapDanhSach(maHoiVien, maGoiTap);
                return View();
            }

            TempData["Error"] = ketQua.Message;
            NapDanhSach(maHoiVien, maGoiTap);
            return View();
        }

        // API kiểm tra tình trạng gói tập của hội viên (cho Ajax gọi từ Create.cshtml)
        [HttpGet]
        public IActionResult KiemTraGoiTap(string maHoiVien, string? maGoiTap)
        {
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
                ngayKetThucHienTai = activeTicket.NgayKetThuc.ToString("dd/MM/yyyy"),
                ngayKetThucHienTaiIso = activeTicket.NgayKetThuc.ToString("yyyy-MM-dd"),
                canhBao = $"Bạn đang trong thời gian sử dụng của gói tập {tenGoiHienTai}, bạn có xác nhận đăng ký gói khác không?"
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
        public IActionResult Huy(string id)
        {
            var res = _service.Huy(id);
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
        public IActionResult GiaHan(string id, int soThang = 1)
        {
            bool isAdmin = User.IsInRole("Admin") || User.IsInRole("admin");
            var res = _service.GiaHan(id, soThang, isHoiVien: !isAdmin);
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

        private void NapDanhSach(
            string? maHoiVien = null,
            string? maGoiTap = null)
        {
            ViewBag.HoiViens = new SelectList(
                _service.GetHoiViens(),
                "MaHoiVien",
                "HoTen",
                maHoiVien);

            ViewBag.GoiTaps = new SelectList(
                _service.GetGoiTaps(),
                "MaGoiTap",
                "TenGoiTap",
                maGoiTap);
        }
    }
}
