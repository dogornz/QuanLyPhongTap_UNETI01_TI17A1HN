using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuanLyPhongTap_UNETI01_TI17A1HN.Services;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Controllers
{
    [Authorize(Roles = "Admin,admin")]
    public class PhieuDangKysController : Controller
    {
        private readonly PhieuDangKyService _service;

        public PhieuDangKysController(PhieuDangKyService service)
        {
            _service = service;
        }

        
        [HttpGet]
        public IActionResult Index(string? maHoiVien, string? maGoiTap, string? trangThai, DateTime? tuNgay, DateTime? denNgay)
        {
            var danhSach = _service.TimKiem(maHoiVien, maGoiTap, trangThai, tuNgay, denNgay);

            ViewBag.MaHoiVien = maHoiVien;
            ViewBag.MaGoiTap = maGoiTap;
            ViewBag.TrangThai = trangThai;
            ViewBag.TuNgay = tuNgay?.ToString("yyyy-MM-dd");
            ViewBag.DenNgay = denNgay?.ToString("yyyy-MM-dd");

            ViewBag.HoiViens = new SelectList(_service.GetHoiViens(), "MaHoiVien", "HoTen", maHoiVien);
            ViewBag.GoiTaps = new SelectList(_service.GetGoiTaps(), "MaGoiTap", "TenGoiTap", maGoiTap);

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
            DateTime ngayBatDau)
        {
            var ketQua = _service.DangKy(
                maHoiVien,
                maGoiTap,
                ngayBatDau);

            if (ketQua.Success)
            {
                TempData["Success"] = ketQua.Message;
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = ketQua.Message;

            NapDanhSach(maHoiVien, maGoiTap);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XacNhan(string id)
        {
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
            var res = _service.GiaHan(id, soThang);
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
