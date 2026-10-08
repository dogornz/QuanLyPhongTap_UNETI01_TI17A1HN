using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LoaiGoiTapController : Controller
    {
        private readonly QuanLyPhongTap_UNETI01_TI17A1HNContext _context;

        public LoaiGoiTapController(QuanLyPhongTap_UNETI01_TI17A1HNContext context)
        {
            _context = context;
        }

        // GET: /LoaiGoiTap or /LoaiGoiTap/Index
        public async Task<IActionResult> Index()
        {
            List<LoaiGoiTap> list;
            try
            {
                // Tự động kiểm tra và tạo bảng nếu chưa có
                await _context.Database.EnsureCreatedAsync();
                list = await _context.LoaiGoiTap.OrderBy(l => l.MaLoaiGoi).ToListAsync();
                if (list == null || list.Count == 0)
                {
                    list = GetDefaultSeedList();
                }
            }
            catch
            {
                // Dự phòng trường hợp CSDL chưa cập nhật cấu trúc mới
                list = GetDefaultSeedList();
            }

            return View(list);
        }

        // POST: /LoaiGoiTap/SaveAjax (Thêm mới hoặc Chỉnh sửa qua Modal)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAjax(int? id, string tenLoaiGoi, string? moTa, string trangThai)
        {
            if (string.IsNullOrWhiteSpace(tenLoaiGoi))
            {
                return Json(new { success = false, message = "Tên loại gói tập không được để trống!" });
            }

            try
            {
                await _context.Database.EnsureCreatedAsync();

                if (id.HasValue && id.Value > 0)
                {
                    // Chỉnh sửa
                    var existing = await _context.LoaiGoiTap.FindAsync(id.Value);
                    if (existing != null)
                    {
                        existing.TenLoaiGoi = tenLoaiGoi.Trim();
                        existing.MoTa = moTa?.Trim();
                        existing.TrangThai = string.Equals(trangThai, "inactive", StringComparison.OrdinalIgnoreCase) ? "Inactive" : "Active";
                        existing.NgayCapNhat = DateTime.Now;
                        _context.Update(existing);
                        await _context.SaveChangesAsync();
                        return Json(new { success = true, message = "Cập nhật loại gói tập thành công!" });
                    }
                }

                // Thêm mới
                var newItem = new LoaiGoiTap
                {
                    TenLoaiGoi = tenLoaiGoi.Trim(),
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

        // POST: /LoaiGoiTap/DeleteAjax (Xóa an toàn)
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

                if (item.SoGoiTrucThuoc > 0)
                {
                    return Json(new
                    {
                        success = false,
                        isFkConstraint = true,
                        count = item.SoGoiTrucThuoc,
                        name = item.TenLoaiGoi,
                        message = $"Loại gói '{item.TenLoaiGoi}' đang có {item.SoGoiTrucThuoc} gói con, không thể xóa do ràng buộc khóa ngoại."
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
                    SoGoiTrucThuoc = 8,
                    NgayCapNhat = new DateTime(2026, 3, 12, 8, 30, 0)
                },
                new LoaiGoiTap
                {
                    MaLoaiGoi = 2,
                    TenLoaiGoi = "HUẤN LUYỆN VIÊN CÁ NHÂN (VIP PT)",
                    MoTa = "Giáo án chuyên sâu 1 kèm 1 với Master Coach, đo InBody định kỳ và thực đơn dinh dưỡng riêng",
                    TrangThai = "Active",
                    SoGoiTrucThuoc = 5,
                    NgayCapNhat = new DateTime(2026, 3, 15, 11, 15, 0)
                },
                new LoaiGoiTap
                {
                    MaLoaiGoi = 3,
                    TenLoaiGoi = "YOGA & GROUP X",
                    MoTa = "Tham gia không giới hạn các lớp Zumba, Yoga Flow, Les Mills BodyPump, Pilates studio",
                    TrangThai = "Active",
                    SoGoiTrucThuoc = 4,
                    NgayCapNhat = new DateTime(2026, 3, 18, 14, 20, 0)
                },
                new LoaiGoiTap
                {
                    MaLoaiGoi = 4,
                    TenLoaiGoi = "BƠI LỘI & SAUNA SPA",
                    MoTa = "Bể bơi bốn mùa nước mặn, phòng xông hơi đá muối Himalaya cao cấp",
                    TrangThai = "Active",
                    SoGoiTrucThuoc = 3,
                    NgayCapNhat = new DateTime(2026, 3, 20, 9, 45, 0)
                },
                new LoaiGoiTap
                {
                    MaLoaiGoi = 5,
                    TenLoaiGoi = "COMBO TOÀN NĂNG ALL-IN-ONE",
                    MoTa = "Full quyền lợi tập luyện, hồ bơi, sauna và 10 buổi PT định hướng cho người mới bắt đầu",
                    TrangThai = "Active",
                    SoGoiTrucThuoc = 2,
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
