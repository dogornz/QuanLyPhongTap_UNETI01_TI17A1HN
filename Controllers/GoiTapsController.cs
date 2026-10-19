using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Controllers
{
    [Authorize(Roles = "Admin,admin")]
    public class GoiTapsController : Controller
    {
        private readonly QuanLyPhongTap_UNETI01_TI17A1HNContext _context;

        public GoiTapsController(QuanLyPhongTap_UNETI01_TI17A1HNContext context)
        {
            _context = context;
        }

        // GET: /GoiTaps or /GoiTaps/Index
        public async Task<IActionResult> Index(string? keyword, int? maLoaiGoi, string? trangThai)
        {
            var query = _context.GoiTap
                .Include(g => g.LoaiGoiTap)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var kw = keyword.Trim().ToLower();
                query = query.Where(g => g.MaGoiTap.ToLower().Contains(kw) || g.TenGoiTap.ToLower().Contains(kw) || g.MoTa.ToLower().Contains(kw));
            }

            if (maLoaiGoi.HasValue && maLoaiGoi > 0)
            {
                query = query.Where(g => g.MaLoaiGoi == maLoaiGoi);
            }

            if (!string.IsNullOrWhiteSpace(trangThai) && trangThai != "all")
            {
                query = query.Where(g => g.TrangThai == trangThai);
            }

            var list = await query.OrderBy(g => g.MaGoiTap).ToListAsync();

            ViewBag.Keyword = keyword;
            ViewBag.SelectedMaLoaiGoi = maLoaiGoi;
            ViewBag.SelectedTrangThai = trangThai;
            ViewBag.LoaiGoiTaps = await _context.LoaiGoiTap.OrderBy(l => l.MaLoaiGoi).ToListAsync();

            return View(list);
        }

        // GET: /GoiTaps/Details/GT01
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var goiTap = await _context.GoiTap
                .Include(g => g.LoaiGoiTap)
                .FirstOrDefaultAsync(g => g.MaGoiTap == id);

            if (goiTap == null)
            {
                return NotFound();
            }

            // Lấy danh sách phiếu đăng ký gần đây của gói này
            ViewBag.RecentPhieuDangKy = await _context.PhieuDangKy
                .Include(p => p.HoiVien)
                .Where(p => p.MaGoiTap == id)
                .OrderByDescending(p => p.NgayDangKy)
                .Take(10)
                .ToListAsync();

            ViewBag.TotalPhieu = await _context.PhieuDangKy.CountAsync(p => p.MaGoiTap == id);

            return View(goiTap);
        }

        // GET: /GoiTaps/Create
        public async Task<IActionResult> Create()
        {
            // Tự động sinh mã gói tập tiếp theo (GT01, GT02... GT16...)
            var allCodes = await _context.GoiTap.Select(g => g.MaGoiTap).ToListAsync();
            int nextNum = 1;
            foreach (var code in allCodes)
            {
                if (code.StartsWith("GT", StringComparison.OrdinalIgnoreCase) && int.TryParse(code[2..], out int n))
                {
                    if (n >= nextNum) nextNum = n + 1;
                }
            }

            var model = new GoiTap
            {
                MaGoiTap = $"GT{nextNum:D2}",
                ThoiHanThang = "1",
                SoBuoiToiDa = 30,
                TrangThai = "Đang áp dụng"
            };

            await PopulateLoaiGoiSelectList();
            return View(model);
        }

        // POST: /GoiTaps/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaGoiTap,TenGoiTap,ThoiHanThang,DonGia,SoBuoiToiDa,MoTa,TrangThai,MaLoaiGoi")] GoiTap goiTap)
        {
            if (string.IsNullOrWhiteSpace(goiTap.MaGoiTap))
            {
                ModelState.AddModelError("MaGoiTap", "Mã gói tập không được để trống!");
            }
            else if (await _context.GoiTap.AnyAsync(g => g.MaGoiTap.ToLower() == goiTap.MaGoiTap.Trim().ToLower()))
            {
                ModelState.AddModelError("MaGoiTap", "Mã gói tập đã tồn tại trong hệ thống!");
            }

            if (string.IsNullOrWhiteSpace(goiTap.TenGoiTap))
            {
                ModelState.AddModelError("TenGoiTap", "Tên gói tập không được để trống!");
            }

            if (goiTap.DonGia < 0)
            {
                ModelState.AddModelError("DonGia", "Đơn giá không thể là số âm!");
            }

            if (ModelState.IsValid)
            {
                goiTap.MaGoiTap = goiTap.MaGoiTap.Trim().ToUpper();
                goiTap.TenGoiTap = goiTap.TenGoiTap.Trim();
                goiTap.MoTa = goiTap.MoTa?.Trim() ?? string.Empty;

                _context.Add(goiTap);
                await _context.SaveChangesAsync();

                // Cập nhật lại số gói trực thuộc của Loại gói tập
                if (goiTap.MaLoaiGoi.HasValue)
                {
                    await UpdateSoGoiTrucThuoc(goiTap.MaLoaiGoi.Value);
                }

                TempData["SuccessMessage"] = $"Thêm mới gói tập '{goiTap.TenGoiTap}' ({goiTap.MaGoiTap}) thành công!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateLoaiGoiSelectList(goiTap.MaLoaiGoi);
            return View(goiTap);
        }

        // GET: /GoiTaps/Edit/GT01
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var goiTap = await _context.GoiTap.FindAsync(id);
            if (goiTap == null)
            {
                return NotFound();
            }

            await PopulateLoaiGoiSelectList(goiTap.MaLoaiGoi);
            return View(goiTap);
        }

        // POST: /GoiTaps/Edit/GT01
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaGoiTap,TenGoiTap,ThoiHanThang,DonGia,SoBuoiToiDa,MoTa,TrangThai,MaLoaiGoi")] GoiTap goiTap)
        {
            if (id != goiTap.MaGoiTap)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(goiTap.TenGoiTap))
            {
                ModelState.AddModelError("TenGoiTap", "Tên gói tập không được để trống!");
            }

            if (goiTap.DonGia < 0)
            {
                ModelState.AddModelError("DonGia", "Đơn giá không thể là số âm!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.GoiTap.FindAsync(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }

                    int? oldMaLoaiGoi = existing.MaLoaiGoi;

                    existing.TenGoiTap = goiTap.TenGoiTap.Trim();
                    existing.ThoiHanThang = goiTap.ThoiHanThang?.Trim() ?? "1";
                    existing.DonGia = goiTap.DonGia;
                    existing.SoBuoiToiDa = goiTap.SoBuoiToiDa;
                    existing.MoTa = goiTap.MoTa?.Trim() ?? string.Empty;
                    existing.TrangThai = goiTap.TrangThai;
                    existing.MaLoaiGoi = goiTap.MaLoaiGoi;

                    await _context.SaveChangesAsync();

                    // Cập nhật lại số gói trực thuộc
                    if (oldMaLoaiGoi.HasValue) await UpdateSoGoiTrucThuoc(oldMaLoaiGoi.Value);
                    if (existing.MaLoaiGoi.HasValue && existing.MaLoaiGoi != oldMaLoaiGoi) await UpdateSoGoiTrucThuoc(existing.MaLoaiGoi.Value);

                    TempData["SuccessMessage"] = $"Cập nhật gói tập '{existing.TenGoiTap}' thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoiTapExists(goiTap.MaGoiTap))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }

            await PopulateLoaiGoiSelectList(goiTap.MaLoaiGoi);
            return View(goiTap);
        }

        // GET: /GoiTaps/Delete/GT01
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var goiTap = await _context.GoiTap
                .Include(g => g.LoaiGoiTap)
                .FirstOrDefaultAsync(m => m.MaGoiTap == id);

            if (goiTap == null)
            {
                return NotFound();
            }

            // Kiểm tra ràng buộc dữ liệu: Phiếu đăng ký liên quan
            int relatedTickets = await _context.PhieuDangKy.CountAsync(p => p.MaGoiTap == id);
            ViewBag.RelatedTickets = relatedTickets;
            ViewBag.CanDelete = relatedTickets == 0;

            return View(goiTap);
        }

        // POST: /GoiTaps/Delete/GT01
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var goiTap = await _context.GoiTap.FindAsync(id);
            if (goiTap != null)
            {
                int relatedTickets = await _context.PhieuDangKy.CountAsync(p => p.MaGoiTap == id);
                if (relatedTickets > 0)
                {
                    TempData["ErrorMessage"] = $"Không thể xóa gói tập '{goiTap.TenGoiTap}' vì đang có {relatedTickets} phiếu đăng ký liên quan! Bạn có thể chuyển trạng thái sang 'Ngừng áp dụng'.";
                    return RedirectToAction(nameof(Index));
                }

                int? maLoai = goiTap.MaLoaiGoi;
                _context.GoiTap.Remove(goiTap);
                await _context.SaveChangesAsync();

                if (maLoai.HasValue)
                {
                    await UpdateSoGoiTrucThuoc(maLoai.Value);
                }

                TempData["SuccessMessage"] = $"Đã xóa gói tập '{goiTap.TenGoiTap}' thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /GoiTaps/ChangeStatus/GT01
        public async Task<IActionResult> ChangeStatus(string id)
        {
            var goiTap = await _context.GoiTap.FindAsync(id);
            if (goiTap == null)
            {
                return NotFound();
            }

            goiTap.TrangThai = goiTap.TrangThai == "Đang áp dụng" ? "Ngừng áp dụng" : "Đang áp dụng";
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Đã chuyển trạng thái gói '{goiTap.TenGoiTap}' thành '{goiTap.TrangThai}'!";
            return RedirectToAction(nameof(Index));
        }

        #region Helpers

        private bool GoiTapExists(string id)
        {
            return _context.GoiTap.Any(e => e.MaGoiTap == id);
        }

        private async Task PopulateLoaiGoiSelectList(int? selectedId = null)
        {
            var list = await _context.LoaiGoiTap.OrderBy(l => l.MaLoaiGoi).ToListAsync();
            ViewBag.MaLoaiGoi = new SelectList(list, "MaLoaiGoi", "TenLoaiGoi", selectedId);
        }

        private async Task UpdateSoGoiTrucThuoc(int maLoaiGoi)
        {
            var loai = await _context.LoaiGoiTap.FindAsync(maLoaiGoi);
            if (loai != null)
            {
                loai.SoGoiTrucThuoc = await _context.GoiTap.CountAsync(g => g.MaLoaiGoi == maLoaiGoi);
                loai.NgayCapNhat = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        #endregion
    }
}
