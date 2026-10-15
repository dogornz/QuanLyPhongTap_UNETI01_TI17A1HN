// Quản lý gói tập (CRUD cho Admin), tìm kiếm, lọc, sắp xếp và phân trang
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyPhongTap_UNETI01_TI17A1HN.Filters;
using QuanLyPhongTap_UNETI01_TI17A1HN.Helpers;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using QuanLyPhongTap_UNETI01_TI17A1HN.ViewModels;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Controllers
{
    public class GoiTapsController : Controller
    {
        private const int KichThuocTrang = 6;
        private readonly QuanLyPhongTap_UNETI01_TI17A1HNContext _context;

        public GoiTapsController(QuanLyPhongTap_UNETI01_TI17A1HNContext context)
        {
            _context = context;
        }

        // GET: GoiTaps?TuKhoa=...&MaLoaiGoi=...&TrangThai=...&GiaTu=...&GiaDen=...&ThoiHanTu=...&ThoiHanDen=...&SapXep=...&Trang=...
        // Admin và Hội viên đều xem được; Hội viên chỉ thấy gói "Đang áp dụng".
        [YeuCauDangNhap]
        public async Task<IActionResult> Index([FromQuery] GoiTapBoLoc boLoc)
        {
            bool laAdmin = HttpContext.Session.LaAdmin();

            if (!SapXepGoiTap.HopLe(boLoc.SapXep)) boLoc.SapXep = SapXepGoiTap.TenAZ;

            // Quy trình: Truy vấn -> Tìm kiếm -> Lọc -> Sắp xếp -> Phân trang -> Hiển thị
            // 1. Truy vấn (chưa chạm vào database, mới chỉ dựng câu LINQ)
            IQueryable<GoiTap> truyVan = _context.GoiTap
                .AsNoTracking()
                .Include(g => g.LoaiGoiTap);

            // 2. Tìm kiếm theo tên gói
            if (!string.IsNullOrWhiteSpace(boLoc.TuKhoa))
            {
                var tuKhoa = boLoc.TuKhoa.Trim();
                truyVan = truyVan.Where(g => g.TenGoiTap.Contains(tuKhoa));
            }

            // 3. Lọc (các điều kiện cộng dồn với nhau bằng AND)
            if (boLoc.MaLoaiGoi.HasValue)
            {
                var maLoai = boLoc.MaLoaiGoi.Value;
                truyVan = truyVan.Where(g => g.MaLoaiGoi == maLoai);
            }

            // Hội viên luôn bị ép về "Đang áp dụng", Admin lọc theo trạng thái tùy chọn
            var trangThai = laAdmin ? boLoc.TrangThai : TrangThaiGoiTap.DangApDung;
            if (!string.IsNullOrEmpty(trangThai))
            {
                truyVan = truyVan.Where(g => g.TrangThai == trangThai);
            }

            if (boLoc.GiaTu.HasValue)
            {
                var giaTu = boLoc.GiaTu.Value;
                truyVan = truyVan.Where(g => g.DonGia >= giaTu);
            }
            if (boLoc.GiaDen.HasValue)
            {
                var giaDen = boLoc.GiaDen.Value;
                truyVan = truyVan.Where(g => g.DonGia <= giaDen);
            }
            if (boLoc.ThoiHanTu.HasValue)
            {
                var tu = boLoc.ThoiHanTu.Value;
                truyVan = truyVan.Where(g => g.ThoiHanThang >= tu);
            }
            if (boLoc.ThoiHanDen.HasValue)
            {
                var den = boLoc.ThoiHanDen.Value;
                truyVan = truyVan.Where(g => g.ThoiHanThang <= den);
            }

            // 4. Sắp xếp. ThenBy(MaGoiTap) để thứ tự luôn xác định, tránh trùng/mất dòng khi chuyển trang.
            truyVan = boLoc.SapXep switch
            {
                SapXepGoiTap.TenZA => truyVan.OrderByDescending(g => g.TenGoiTap).ThenBy(g => g.MaGoiTap),
                SapXepGoiTap.GiaTang => truyVan.OrderBy(g => g.DonGia).ThenBy(g => g.MaGoiTap),
                SapXepGoiTap.GiaGiam => truyVan.OrderByDescending(g => g.DonGia).ThenBy(g => g.MaGoiTap),
                SapXepGoiTap.ThoiHanTang => truyVan.OrderBy(g => g.ThoiHanThang).ThenBy(g => g.MaGoiTap),
                SapXepGoiTap.ThoiHanGiam => truyVan.OrderByDescending(g => g.ThoiHanThang).ThenBy(g => g.MaGoiTap),
                _ => truyVan.OrderBy(g => g.TenGoiTap).ThenBy(g => g.MaGoiTap)
            };

            // 5. Phân trang bằng Skip()/Take() trên SQL Server (không tải hết dữ liệu về rồi cắt)
            var ketQua = await PhanTrangDanhSach<GoiTap>.TaoAsync(truyVan, boLoc.Trang, KichThuocTrang);
            boLoc.Trang = ketQua.TrangHienTai;

            // 6. Hiển thị
            var viewModel = new GoiTapIndexViewModel
            {
                BoLoc = boLoc,
                KetQua = ketQua,
                LaAdmin = laAdmin,
                DanhSachLoaiGoi = await _context.LoaiGoiTap.AsNoTracking().OrderBy(l => l.TenLoaiGoi).ToListAsync()
            };
            return View(viewModel);
        }

        // GET: GoiTaps/Details/5
        [YeuCauDangNhap]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var goiTap = await _context.GoiTap
                .AsNoTracking()
                .Include(g => g.LoaiGoiTap)
                .FirstOrDefaultAsync(g => g.MaGoiTap == id.Value);
            if (goiTap == null) return NotFound();

            bool laAdmin = HttpContext.Session.LaAdmin();
            // Hội viên không được xem chi tiết gói tạm ngừng / ngừng áp dụng
            if (!laAdmin && goiTap.TrangThai != TrangThaiGoiTap.DangApDung) return NotFound();

            ViewBag.LaAdmin = laAdmin;
            return View(goiTap);
        }

        // GET: GoiTaps/Create
        [YeuCauAdmin]
        public async Task<IActionResult> Create()
        {
            await NapDanhSachChonAsync();
            return View(new GoiTap());
        }

        // POST: GoiTaps/Create
        // [Bind] liệt kê rõ các trường được nhận từ Form, KHÔNG có MaGoiTap và LoaiGoiTap (chống overposting).
        [HttpPost]
        [ValidateAntiForgeryToken]
        [YeuCauAdmin]
        public async Task<IActionResult> Create([Bind("TenGoiTap,MaLoaiGoi,ThoiHanThang,DonGia,SoBuoiToiDa,MoTa,TrangThai")] GoiTap goiTap)
        {
            await KiemTraNghiepVuAsync(goiTap);

            if (ModelState.IsValid)
            {
                _context.Add(goiTap);
                await _context.SaveChangesAsync();
                TempData["ThongBao"] = $"Đã thêm gói tập \"{goiTap.TenGoiTap}\".";
                return RedirectToAction(nameof(Index));
            }

            await NapDanhSachChonAsync(goiTap);
            return View(goiTap);
        }

        // GET: GoiTaps/Edit/5
        [YeuCauAdmin]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var goiTap = await _context.GoiTap.FindAsync(id.Value);
            if (goiTap == null) return NotFound();

            await NapDanhSachChonAsync(goiTap);
            return View(goiTap);
        }

        // POST: GoiTaps/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [YeuCauAdmin]
        public async Task<IActionResult> Edit(int id, [Bind("MaGoiTap,TenGoiTap,MaLoaiGoi,ThoiHanThang,DonGia,SoBuoiToiDa,MoTa,TrangThai")] GoiTap goiTap)
        {
            if (id != goiTap.MaGoiTap) return NotFound();

            await KiemTraNghiepVuAsync(goiTap);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(goiTap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.GoiTap.AnyAsync(e => e.MaGoiTap == goiTap.MaGoiTap)) return NotFound();
                    throw;
                }

                TempData["ThongBao"] = $"Đã cập nhật gói tập \"{goiTap.TenGoiTap}\".";
                return RedirectToAction(nameof(Index));
            }

            await NapDanhSachChonAsync(goiTap);
            return View(goiTap);
        }

        // GET: GoiTaps/Delete/5
        [YeuCauAdmin]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var goiTap = await _context.GoiTap
                .AsNoTracking()
                .Include(g => g.LoaiGoiTap)
                .FirstOrDefaultAsync(g => g.MaGoiTap == id.Value);
            if (goiTap == null) return NotFound();

            return View(goiTap);
        }

        // POST: GoiTaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [YeuCauAdmin]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goiTap = await _context.GoiTap.FindAsync(id);
            if (goiTap == null) return NotFound();

            try
            {
                _context.GoiTap.Remove(goiTap);
                await _context.SaveChangesAsync();
                TempData["ThongBao"] = $"Đã xóa gói tập \"{goiTap.TenGoiTap}\".";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                // Gói đã có phiếu đăng ký tham chiếu tới (khóa ngoại của Module 3 phải để Restrict) thì không xóa được.
                TempData["Loi"] = "Không thể xóa gói tập này vì đã có phiếu đăng ký liên quan. " +
                                  "Hãy chuyển trạng thái sang \"Ngừng áp dụng\" thay vì xóa.";
                return RedirectToAction(nameof(Delete), new { id });
            }
        }

        // ---------------------------------------------------------------------------------
        // Kiểm tra nghiệp vụ ngoài Data Annotation: trạng thái hợp lệ, loại gói có tồn tại trong database.
        private async Task KiemTraNghiepVuAsync(GoiTap goiTap)
        {
            if (!string.IsNullOrEmpty(goiTap.TrangThai) && !TrangThaiGoiTap.DanhSach.Contains(goiTap.TrangThai))
            {
                ModelState.AddModelError(nameof(GoiTap.TrangThai), "Trạng thái không hợp lệ");
            }

            if (goiTap.MaLoaiGoi > 0 && !await _context.LoaiGoiTap.AnyAsync(l => l.MaLoaiGoi == goiTap.MaLoaiGoi))
            {
                ModelState.AddModelError(nameof(GoiTap.MaLoaiGoi), "Loại gói không tồn tại");
            }
        }

        // Nạp dữ liệu cho 2 thẻ <select> (Loại gói lấy từ database, Trạng thái lấy từ danh sách cố định)
        private async Task NapDanhSachChonAsync(GoiTap? goiTap = null)
        {
            var dsLoaiGoi = await _context.LoaiGoiTap.AsNoTracking().OrderBy(l => l.TenLoaiGoi).ToListAsync();
            ViewBag.DanhSachLoaiGoi = new SelectList(dsLoaiGoi, nameof(LoaiGoiTap.MaLoaiGoi), nameof(LoaiGoiTap.TenLoaiGoi), goiTap?.MaLoaiGoi);
            ViewBag.DanhSachTrangThai = new SelectList(TrangThaiGoiTap.DanhSach, goiTap?.TrangThai);
        }
    }
}
