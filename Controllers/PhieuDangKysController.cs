
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuanLyPhongTap_UNETI01_TI17A1HN.Services;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Controllers
{
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
