using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using QuanLyPhongTap_UNETI01_TI17A1HN.ViewModels;

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

        // GET: /GoiTaps
        // Quy trình: Truy vấn dữ liệu -> Tìm kiếm -> Lọc -> Sắp xếp -> Phân trang -> Hiển thị
        public async Task<IActionResult> Index(
            string? searchName,
            int? maLoaiGoi,
            string? trangThai,
            decimal? giaTu,
            decimal? giaDen,
            string? thoiHan,
            string? sortOrder,
            int page = 1)
        {
            // 1. Truy vấn dữ liệu
            var query = _context.GoiTap
                .Include(g => g.LoaiGoiTap)
                .AsQueryable();

            // 2. Tìm kiếm (6.3: Cho phép tìm kiếm theo Tên gói tập. Thực hiện bằng LINQ)
            if (!string.IsNullOrWhiteSpace(searchName))
            {
                var kw = searchName.Trim().ToLower();
                query = query.Where(g => g.TenGoiTap.ToLower().Contains(kw));
            }

            // 3. Lọc (6.4: Loại gói, Trạng thái, Khoảng đơn giá, Thời hạn. Kết hợp được với nhau)
            if (maLoaiGoi.HasValue && maLoaiGoi > 0)
            {
                query = query.Where(g => g.MaLoaiGoi == maLoaiGoi.Value);
            }

            if (!string.IsNullOrWhiteSpace(trangThai) && trangThai != "all")
            {
                query = query.Where(g => g.TrangThai == trangThai);
            }

            if (giaTu.HasValue && giaTu > 0)
            {
                query = query.Where(g => g.DonGia >= giaTu.Value);
            }

            if (giaDen.HasValue && giaDen > 0)
            {
                query = query.Where(g => g.DonGia <= giaDen.Value);
            }

            if (!string.IsNullOrWhiteSpace(thoiHan) && thoiHan != "all")
            {
                query = query.Where(g => g.ThoiHanThang == thoiHan);
            }

            // 4. Sắp xếp (6.5: Tên gói A->Z, Z->A, Đơn giá tăng/giảm dần, Thời hạn tăng/giảm dần)
            query = sortOrder switch
            {
                "ten_desc" => query.OrderByDescending(g => g.TenGoiTap),
                "gia_asc" => query.OrderBy(g => g.DonGia),
                "gia_desc" => query.OrderByDescending(g => g.DonGia),
                "thoihan_asc" => query.OrderBy(g => Convert.ToInt32(g.ThoiHanThang)),
                "thoihan_desc" => query.OrderByDescending(g => Convert.ToInt32(g.ThoiHanThang)),
                "ten_asc" => query.OrderBy(g => g.TenGoiTap),
                _ => query.OrderBy(g => g.TenGoiTap)
            };

            // 5. Phân trang (6.6: Thực hiện bằng EF Core / LINQ Skip() và Take())
            int totalItems = await query.CountAsync();
            int pageSize = 5; // Số phần tử mỗi trang
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            if (totalPages < 1) totalPages = 1;
            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // 6. Hiển thị
            var loaiGoiList = await _context.LoaiGoiTap.OrderBy(l => l.MaLoaiGoi).ToListAsync();
            var rawThoiHan = await _context.GoiTap
                .Select(g => g.ThoiHanThang)
                .Where(t => !string.IsNullOrEmpty(t))
                .Distinct()
                .ToListAsync();
            var thoiHanList = rawThoiHan
                .OrderBy(t => int.TryParse(t, out int val) ? val : 0)
                .ToList();

            var viewModel = new GoiTapIndexViewModel
            {
                Items = items,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                PageSize = pageSize,
                SearchName = searchName,
                MaLoaiGoi = maLoaiGoi,
                TrangThai = trangThai,
                GiaTu = giaTu,
                GiaDen = giaDen,
                ThoiHan = thoiHan,
                SortOrder = sortOrder ?? "ten_asc",
                LoaiGoiList = loaiGoiList,
                ThoiHanList = thoiHanList
            };

            return View(viewModel);
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
            // 6.2: Kiểm tra các yêu cầu Create
            if (string.IsNullOrWhiteSpace(goiTap.MaGoiTap))
            {
                ModelState.AddModelError("MaGoiTap", "Mã gói tập không được để trống!");
            }
            else if (await _context.GoiTap.AnyAsync(g => g.MaGoiTap.ToLower() == goiTap.MaGoiTap.Trim().ToLower()))
            {
                ModelState.AddModelError("MaGoiTap", "Mã gói tập đã tồn tại trong hệ thống!");
            }

            // Tên gói bắt buộc
            if (string.IsNullOrWhiteSpace(goiTap.TenGoiTap))
            {
                ModelState.AddModelError("TenGoiTap", "Tên gói tập không được để trống!");
            }

            // Loại gói sử dụng select lấy dữ liệu từ Database
            if (!goiTap.MaLoaiGoi.HasValue || !await _context.LoaiGoiTap.AnyAsync(l => l.MaLoaiGoi == goiTap.MaLoaiGoi.Value))
            {
                ModelState.AddModelError("MaLoaiGoi", "Vui lòng chọn loại gói tập hợp lệ từ danh sách!");
            }

            // Thời hạn > 0
            if (string.IsNullOrWhiteSpace(goiTap.ThoiHanThang) || !int.TryParse(goiTap.ThoiHanThang, out int thoiHanVal) || thoiHanVal <= 0)
            {
                ModelState.AddModelError("ThoiHanThang", "Thời hạn phải lớn hơn 0 tháng!");
            }

            // Đơn giá > 0
            if (goiTap.DonGia <= 0)
            {
                ModelState.AddModelError("DonGia", "Đơn giá phải lớn hơn 0!");
            }

            // Số buổi tối đa > 0 nếu gói có giới hạn số buổi
            if (goiTap.SoBuoiToiDa < 0)
            {
                ModelState.AddModelError("SoBuoiToiDa", "Số buổi tối đa không được nhỏ hơn 0!");
            }

            // Trạng thái hợp lệ
            if (string.IsNullOrWhiteSpace(goiTap.TrangThai) ||
                (goiTap.TrangThai != "Đang áp dụng" && goiTap.TrangThai != "Tạm ngừng" && goiTap.TrangThai != "Ngừng áp dụng"))
            {
                ModelState.AddModelError("TrangThai", "Trạng thái không hợp lệ!");
            }

            if (ModelState.IsValid)
            {
                goiTap.MaGoiTap = goiTap.MaGoiTap.Trim().ToUpper();
                goiTap.TenGoiTap = goiTap.TenGoiTap.Trim();
                goiTap.ThoiHanThang = goiTap.ThoiHanThang.Trim();
                goiTap.MoTa = goiTap.MoTa?.Trim() ?? string.Empty;

                _context.Add(goiTap);
                await _context.SaveChangesAsync();

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

            // Tên gói bắt buộc
            if (string.IsNullOrWhiteSpace(goiTap.TenGoiTap))
            {
                ModelState.AddModelError("TenGoiTap", "Tên gói tập không được để trống!");
            }

            // Loại gói sử dụng select lấy dữ liệu từ Database
            if (!goiTap.MaLoaiGoi.HasValue || !await _context.LoaiGoiTap.AnyAsync(l => l.MaLoaiGoi == goiTap.MaLoaiGoi.Value))
            {
                ModelState.AddModelError("MaLoaiGoi", "Vui lòng chọn loại gói tập hợp lệ từ danh sách!");
            }

            // Thời hạn > 0
            if (string.IsNullOrWhiteSpace(goiTap.ThoiHanThang) || !int.TryParse(goiTap.ThoiHanThang, out int thoiHanVal) || thoiHanVal <= 0)
            {
                ModelState.AddModelError("ThoiHanThang", "Thời hạn phải lớn hơn 0 tháng!");
            }

            // Đơn giá > 0
            if (goiTap.DonGia <= 0)
            {
                ModelState.AddModelError("DonGia", "Đơn giá phải lớn hơn 0!");
            }

            // Số buổi tối đa > 0 nếu gói có giới hạn số buổi
            if (goiTap.SoBuoiToiDa < 0)
            {
                ModelState.AddModelError("SoBuoiToiDa", "Số buổi tối đa không được nhỏ hơn 0!");
            }

            // Trạng thái hợp lệ
            if (string.IsNullOrWhiteSpace(goiTap.TrangThai) ||
                (goiTap.TrangThai != "Đang áp dụng" && goiTap.TrangThai != "Tạm ngừng" && goiTap.TrangThai != "Ngừng áp dụng"))
            {
                ModelState.AddModelError("TrangThai", "Trạng thái không hợp lệ!");
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
                    existing.ThoiHanThang = goiTap.ThoiHanThang.Trim();
                    existing.DonGia = goiTap.DonGia;
                    existing.SoBuoiToiDa = goiTap.SoBuoiToiDa;
                    existing.MoTa = goiTap.MoTa?.Trim() ?? string.Empty;
                    existing.TrangThai = goiTap.TrangThai;
                    existing.MaLoaiGoi = goiTap.MaLoaiGoi;

                    await _context.SaveChangesAsync();

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

            goiTap.TrangThai = goiTap.TrangThai switch
            {
                "Đang áp dụng" => "Tạm ngừng",
                "Tạm ngừng" => "Ngừng áp dụng",
                _ => "Đang áp dụng"
            };
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
