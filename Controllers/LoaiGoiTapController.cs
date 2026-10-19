using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Controllers
{
    [Authorize(Roles = "Admin,admin")]
    public class LoaiGoiTapController : Controller
    {
        private readonly QuanLyPhongTap_UNETI01_TI17A1HNContext _context;

        public LoaiGoiTapController(QuanLyPhongTap_UNETI01_TI17A1HNContext context)
        {
            _context = context;
        }

        // GET: LoaiGoiTap or LoaiGoiTap/Index
        public async Task<IActionResult> Index()
        {
            await _context.Database.EnsureCreatedAsync();

            var list = await _context.LoaiGoiTap.OrderBy(l => l.MaLoaiGoi).ToListAsync();
            if (list == null || list.Count == 0)
            {
                list = GetDefaultSeedList();
                _context.LoaiGoiTap.AddRange(list);
                await _context.SaveChangesAsync();
            }

            // Đồng bộ số gói tập trực thuộc từ bảng GoiTap
            foreach (var item in list)
            {
                var relatedCount = await _context.GoiTap.CountAsync(g => g.MaLoaiGoi == item.MaLoaiGoi);
                if (relatedCount > 0 || item.SoGoiTrucThuoc == 0)
                {
                    item.SoGoiTrucThuoc = relatedCount;
                }
            }

            return View(list);
        }

        // GET: LoaiGoiTap/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiGoiTap = await _context.LoaiGoiTap
                .FirstOrDefaultAsync(m => m.MaLoaiGoi == id);

            if (loaiGoiTap == null)
            {
                return NotFound();
            }

            // Lấy danh sách gói tập trực thuộc
            var relatedGoiTap = await _context.GoiTap
                .Where(g => g.MaLoaiGoi == id)
                .ToListAsync();

            if (relatedGoiTap.Count > 0 || loaiGoiTap.SoGoiTrucThuoc == 0)
            {
                loaiGoiTap.SoGoiTrucThuoc = relatedGoiTap.Count;
            }

            ViewBag.RelatedGoiTap = relatedGoiTap;

            return View(loaiGoiTap);
        }

        // GET: LoaiGoiTap/Create
        public IActionResult Create()
        {
            return View(new LoaiGoiTap { TrangThai = "Active" });
        }

        // POST: LoaiGoiTap/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLoaiGoi,TenLoaiGoi,MoTa,TrangThai")] LoaiGoiTap loaiGoiTap)
        {
            var trimmedName = loaiGoiTap.TenLoaiGoi?.Trim();

            // Yêu cầu: Tên loại gói bắt buộc
            if (string.IsNullOrWhiteSpace(trimmedName))
            {
                ModelState.AddModelError("TenLoaiGoi", "Tên loại gói là bắt buộc.");
            }
            else
            {
                // Yêu cầu: Tên loại gói không được trùng
                var isDuplicate = await _context.LoaiGoiTap.AnyAsync(l => l.TenLoaiGoi.ToLower() == trimmedName.ToLower());
                if (isDuplicate)
                {
                    ModelState.AddModelError("TenLoaiGoi", "Tên loại gói tập đã tồn tại trong hệ thống. Vui lòng nhập tên khác!");
                }
            }

            if (ModelState.IsValid)
            {
                loaiGoiTap.TenLoaiGoi = trimmedName!;
                loaiGoiTap.MoTa = loaiGoiTap.MoTa?.Trim();
                loaiGoiTap.SoGoiTrucThuoc = 0;
                loaiGoiTap.NgayCapNhat = DateTime.Now;

                _context.Add(loaiGoiTap);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Thêm mới loại gói tập '{loaiGoiTap.TenLoaiGoi}' thành công!";
                return RedirectToAction(nameof(Index));
            }

            return View(loaiGoiTap);
        }

        // GET: LoaiGoiTap/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiGoiTap = await _context.LoaiGoiTap.FindAsync(id);
            if (loaiGoiTap == null)
            {
                return NotFound();
            }

            return View(loaiGoiTap);
        }

        // POST: LoaiGoiTap/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaLoaiGoi,TenLoaiGoi,MoTa,TrangThai")] LoaiGoiTap loaiGoiTap)
        {
            if (id != loaiGoiTap.MaLoaiGoi)
            {
                return NotFound();
            }

            var trimmedName = loaiGoiTap.TenLoaiGoi?.Trim();

            // Yêu cầu: Tên loại gói bắt buộc
            if (string.IsNullOrWhiteSpace(trimmedName))
            {
                ModelState.AddModelError("TenLoaiGoi", "Tên loại gói là bắt buộc.");
            }
            else
            {
                // Yêu cầu: Tên loại gói không được trùng (ngoại trừ chính nó)
                var isDuplicate = await _context.LoaiGoiTap.AnyAsync(l => l.MaLoaiGoi != id && l.TenLoaiGoi.ToLower() == trimmedName.ToLower());
                if (isDuplicate)
                {
                    ModelState.AddModelError("TenLoaiGoi", "Tên loại gói tập đã tồn tại trong hệ thống. Vui lòng nhập tên khác!");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.LoaiGoiTap.FindAsync(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }

                    existing.TenLoaiGoi = trimmedName!;
                    existing.MoTa = loaiGoiTap.MoTa?.Trim();
                    existing.TrangThai = loaiGoiTap.TrangThai;
                    existing.NgayCapNhat = DateTime.Now;

                    _context.Update(existing);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Cập nhật loại gói tập '{existing.TenLoaiGoi}' thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiGoiTapExists(loaiGoiTap.MaLoaiGoi))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(loaiGoiTap);
        }

        // GET: LoaiGoiTap/ChangeStatus/5
        public async Task<IActionResult> ChangeStatus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiGoiTap = await _context.LoaiGoiTap.FindAsync(id);
            if (loaiGoiTap == null)
            {
                return NotFound();
            }

            // Đảo trạng thái Active <-> Inactive
            if (string.Equals(loaiGoiTap.TrangThai, "Active", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(loaiGoiTap.TrangThai, "Unlock", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(loaiGoiTap.TrangThai, "Hoạt động", StringComparison.OrdinalIgnoreCase))
            {
                loaiGoiTap.TrangThai = "Inactive";
            }
            else
            {
                loaiGoiTap.TrangThai = "Active";
            }

            loaiGoiTap.NgayCapNhat = DateTime.Now;
            _context.Update(loaiGoiTap);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Đã cập nhật trạng thái của loại gói '{loaiGoiTap.TenLoaiGoi}' sang: {loaiGoiTap.TrangThai}!";
            return RedirectToAction(nameof(Index));
        }

        // GET: LoaiGoiTap/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiGoiTap = await _context.LoaiGoiTap
                .FirstOrDefaultAsync(m => m.MaLoaiGoi == id);
            if (loaiGoiTap == null)
            {
                return NotFound();
            }

            // Kiểm tra các gói tập liên quan
            var relatedGoiTap = await _context.GoiTap
                .Where(g => g.MaLoaiGoi == id)
                .ToListAsync();

            var relatedCount = relatedGoiTap.Count > 0 ? relatedGoiTap.Count : loaiGoiTap.SoGoiTrucThuoc;
            bool canDelete = (relatedCount == 0);

            ViewBag.CanDelete = canDelete;
            ViewBag.RelatedCount = relatedCount;
            ViewBag.RelatedGoiTap = relatedGoiTap;

            return View(loaiGoiTap);
        }

        // POST: LoaiGoiTap/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id, int? maLoaiGoi)
        {
            var targetId = id ?? maLoaiGoi;
            if (targetId == null)
            {
                return NotFound();
            }

            var loaiGoiTap = await _context.LoaiGoiTap.FindAsync(targetId);
            if (loaiGoiTap == null)
            {
                return NotFound();
            }

            // Yêu cầu: Không được xóa loại gói nếu việc xóa làm dữ liệu GoiTap liên quan không còn hợp lệ
            var hasRelatedGoiTap = await _context.GoiTap.AnyAsync(g => g.MaLoaiGoi == targetId) || loaiGoiTap.SoGoiTrucThuoc > 0;
            if (hasRelatedGoiTap)
            {
                TempData["ErrorMessage"] = $"Không thể xóa loại gói '{loaiGoiTap.TenLoaiGoi}' vì đang có dữ liệu GoiTap trực thuộc liên quan! Vui lòng xóa hoặc chuyển các gói tập liên quan trước.";
                return RedirectToAction(nameof(Index));
            }

            _context.LoaiGoiTap.Remove(loaiGoiTap);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Đã xóa loại gói tập '{loaiGoiTap.TenLoaiGoi}' thành công!";
            return RedirectToAction(nameof(Index));
        }

        // POST: /LoaiGoiTap/SaveAjax (Hỗ trợ gọi Ajax nếu cần)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAjax(int? id, string tenLoaiGoi, string? moTa, string trangThai)
        {
            var trimmedName = tenLoaiGoi?.Trim();
            if (string.IsNullOrWhiteSpace(trimmedName))
            {
                return Json(new { success = false, message = "Tên loại gói tập không được để trống!" });
            }

            try
            {
                if (id.HasValue && id.Value > 0)
                {
                    var isDuplicate = await _context.LoaiGoiTap.AnyAsync(l => l.MaLoaiGoi != id.Value && l.TenLoaiGoi.ToLower() == trimmedName.ToLower());
                    if (isDuplicate)
                    {
                        return Json(new { success = false, message = "Tên loại gói tập đã tồn tại trong hệ thống!" });
                    }

                    var existing = await _context.LoaiGoiTap.FindAsync(id.Value);
                    if (existing != null)
                    {
                        existing.TenLoaiGoi = trimmedName;
                        existing.MoTa = moTa?.Trim();
                        existing.TrangThai = string.Equals(trangThai, "inactive", StringComparison.OrdinalIgnoreCase) ? "Inactive" : "Active";
                        existing.NgayCapNhat = DateTime.Now;
                        _context.Update(existing);
                        await _context.SaveChangesAsync();
                        return Json(new { success = true, message = "Cập nhật loại gói tập thành công!" });
                    }
                }

                var isDuplicateNew = await _context.LoaiGoiTap.AnyAsync(l => l.TenLoaiGoi.ToLower() == trimmedName.ToLower());
                if (isDuplicateNew)
                {
                    return Json(new { success = false, message = "Tên loại gói tập đã tồn tại trong hệ thống!" });
                }

                var newItem = new LoaiGoiTap
                {
                    TenLoaiGoi = trimmedName,
                    MoTa = moTa?.Trim(),
                    TrangThai = string.Equals(trangThai, "inactive", StringComparison.OrdinalIgnoreCase) ? "Inactive" : "Active",
                    SoGoiTrucThuoc = 0,
                    NgayCapNhat = DateTime.Now
                };

                _context.LoaiGoiTap.Add(newItem);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Thêm loại gói tập mới thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi lưu dữ liệu: " + ex.Message });
            }
        }

        // POST: /LoaiGoiTap/DeleteAjax (Hỗ trợ gọi Ajax nếu cần)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            try
            {
                var item = await _context.LoaiGoiTap.FindAsync(id);
                if (item == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy loại gói tập này." });
                }

                var hasRelated = await _context.GoiTap.AnyAsync(g => g.MaLoaiGoi == id) || item.SoGoiTrucThuoc > 0;
                if (hasRelated)
                {
                    return Json(new
                    {
                        success = false,
                        isFkConstraint = true,
                        count = item.SoGoiTrucThuoc,
                        name = item.TenLoaiGoi,
                        message = $"Loại gói '{item.TenLoaiGoi}' đang có các gói tập liên quan, không thể xóa do ràng buộc tính hợp lệ của dữ liệu GoiTap."
                    });
                }

                _context.LoaiGoiTap.Remove(item);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Đã xóa loại gói tập thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi xóa: " + ex.Message });
            }
        }

        private bool LoaiGoiTapExists(int id)
        {
            return _context.LoaiGoiTap.Any(e => e.MaLoaiGoi == id);
        }

        private List<LoaiGoiTap> GetDefaultSeedList()
        {
            return new List<LoaiGoiTap>
            {
                new LoaiGoiTap
                {
                    MaLoaiGoi = 1,
                    TenLoaiGoi = "GYM & CARDIO TIÊU CHUẨN",
                    MoTa = "Truy cập khu tập tạ tự do, máy cardio hiện đại và phòng tắm nóng lạnh tiêu chuẩn quốc tế",
                    TrangThai = "Active",
                    SoGoiTrucThuoc = 4,
                    NgayCapNhat = new DateTime(2026, 3, 12, 8, 30, 0)
                },
                new LoaiGoiTap
                {
                    MaLoaiGoi = 2,
                    TenLoaiGoi = "HUẤN LUYỆN VIÊN CÁ NHÂN (VIP PT)",
                    MoTa = "Giáo án chuyên sâu 1 kèm 1 với Master Coach, đo InBody định kỳ và thực đơn dinh dưỡng riêng",
                    TrangThai = "Active",
                    SoGoiTrucThuoc = 2,
                    NgayCapNhat = new DateTime(2026, 3, 15, 11, 15, 0)
                },
                new LoaiGoiTap
                {
                    MaLoaiGoi = 3,
                    TenLoaiGoi = "YOGA & GROUP X",
                    MoTa = "Tham gia không giới hạn các lớp Zumba, Yoga Flow, Les Mills BodyPump, Pilates studio",
                    TrangThai = "Active",
                    SoGoiTrucThuoc = 2,
                    NgayCapNhat = new DateTime(2026, 3, 18, 14, 20, 0)
                },
                new LoaiGoiTap
                {
                    MaLoaiGoi = 4,
                    TenLoaiGoi = "BƠI LỘI & SAUNA SPA",
                    MoTa = "Bể bơi bốn mùa nước mặn, phòng xông hơi đá muối Himalaya cao cấp",
                    TrangThai = "Active",
                    SoGoiTrucThuoc = 2,
                    NgayCapNhat = new DateTime(2026, 3, 20, 9, 45, 0)
                },
                new LoaiGoiTap
                {
                    MaLoaiGoi = 5,
                    TenLoaiGoi = "COMBO TOÀN NĂNG ALL-IN-ONE",
                    MoTa = "Full quyền lợi tập luyện, hồ bơi, sauna và 10 buổi PT định hướng cho người mới bắt đầu",
                    TrangThai = "Active",
                    SoGoiTrucThuoc = 3,
                    NgayCapNhat = new DateTime(2026, 3, 25, 16, 0, 0)
                },
                new LoaiGoiTap
                {
                    MaLoaiGoi = 6,
                    TenLoaiGoi = "GÓI TRẢI NGHIỆM HÈ (CŨ)",
                    MoTa = "Áp dụng chiến dịch hè (Đã dừng tuyển sinh mới)",
                    TrangThai = "Inactive",
                    SoGoiTrucThuoc = 0,
                    NgayCapNhat = new DateTime(2026, 1, 2, 10, 0, 0)
                }
            };
        }
    }
}
