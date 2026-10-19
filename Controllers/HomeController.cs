using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Controllers
{
    public class HomeController : Controller
    {
        private readonly QuanLyPhongTap_UNETI01_TI17A1HNContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(QuanLyPhongTap_UNETI01_TI17A1HNContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /Home/Index
        public async Task<IActionResult> Index()
        {
            var viewModel = new MemberDashboardViewModel();

            try
            {
                // Lấy toàn bộ danh sách gói tập để hiển thị
                viewModel.DanhSachGoiTap = await _context.GoiTap
                    .OrderBy(g => g.MaGoiTap)
                    .ToListAsync();

                // Nếu hội viên đã đăng nhập, tải thông tin cá nhân và gói đã đăng ký
                if (User.Identity != null && User.Identity.IsAuthenticated)
                {
                    var taiKhoan = await GetCurrentTaiKhoanAsync();
                    if (taiKhoan != null)
                    {
                        viewModel.TaiKhoan = taiKhoan;
                        var hoiVien = await GetOrCreateHoiVienAsync(taiKhoan);
                        viewModel.HoiVien = hoiVien;

                        // Tải các phiếu đăng ký thuộc về chính hội viên này
                        viewModel.DanhSachPhieuDangKy = await _context.PhieuDangKy
                            .Include(p => p.GoiTap)
                            .Include(p => p.HoiVien)
                            .Where(p => p.MaHoiVien == hoiVien.MaHoiVien)
                            .OrderByDescending(p => p.NgayDangKy)
                            .ToListAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tải trang chủ hội viên");
            }

            return View(viewModel);
        }

        // POST: /Home/DangKyGoiTap (Chức năng: Đăng ký gói tập)
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangKyGoiTap([FromForm] DangKyGoiTapDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.MaGoiTap))
            {
                return Json(new { success = false, message = "Vui lòng chọn gói tập hợp lệ!" });
            }

            try
            {
                var taiKhoan = await GetCurrentTaiKhoanAsync();
                if (taiKhoan == null)
                {
                    return Json(new { success = false, message = "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại!" });
                }

                var hoiVien = await GetOrCreateHoiVienAsync(taiKhoan);

                var goiTap = await _context.GoiTap.FirstOrDefaultAsync(g => g.MaGoiTap == dto.MaGoiTap);
                if (goiTap == null)
                {
                    return Json(new { success = false, message = "Gói tập không tồn tại trong hệ thống!" });
                }

                // Tính toán ngày bắt đầu và ngày kết thúc
                DateTime ngayBatDau = dto.NgayBatDau.HasValue ? dto.NgayBatDau.Value.Date : DateTime.Today;

                int months = ParseMonths(goiTap.ThoiHanThang, goiTap.TenGoiTap);
                DateTime ngayKetThuc = ngayBatDau.AddMonths(months > 0 ? months : 1);

                // Tạo mã phiếu đăng ký mới tự động (PDK01, PDK02... PDK31...)
                var existingPdkCodes = await _context.PhieuDangKy
                    .Select(p => p.MaPhieu)
                    .ToListAsync();

                int nextNum = 1;
                foreach (var code in existingPdkCodes)
                {
                    if (code.StartsWith("PDK", StringComparison.OrdinalIgnoreCase) && int.TryParse(code[3..], out int n))
                    {
                        if (n >= nextNum) nextNum = n + 1;
                    }
                }
                string newMaPhieu = $"PDK{nextNum:D2}";

                var newPhieu = new PhieuDangKy
                {
                    MaPhieu = newMaPhieu,
                    MaHoiVien = hoiVien.MaHoiVien,
                    MaGoiTap = goiTap.MaGoiTap,
                    NgayDangKy = DateTime.Now,
                    NgayBatDau = ngayBatDau,
                    NgayKetThuc = ngayKetThuc,
                    DonGia = goiTap.DonGia,
                    TongTien = goiTap.DonGia,
                    TrangThai = "Đang hiệu lực"
                };

                _context.PhieuDangKy.Add(newPhieu);
                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = $"Đăng ký thành công gói '{goiTap.TenGoiTap}'!",
                    maPhieu = newMaPhieu,
                    tenGoi = goiTap.TenGoiTap,
                    ngayBatDau = ngayBatDau.ToString("dd/MM/yyyy"),
                    ngayKetThuc = ngayKetThuc.ToString("dd/MM/yyyy"),
                    tongTien = goiTap.DonGia.ToString("N0") + " đ"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đăng ký gói tập");
                return Json(new { success = false, message = "Đã xảy ra lỗi trong quá trình đăng ký: " + ex.Message });
            }
        }

        // POST: /Home/HuyDangKy (Hủy phiếu đăng ký)
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HuyDangKy([FromForm] string maPhieu)
        {
            if (string.IsNullOrWhiteSpace(maPhieu))
            {
                return Json(new { success = false, message = "Mã phiếu không hợp lệ!" });
            }

            try
            {
                var taiKhoan = await GetCurrentTaiKhoanAsync();
                if (taiKhoan == null)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập!" });
                }

                var hoiVien = await GetOrCreateHoiVienAsync(taiKhoan);

                var phieu = await _context.PhieuDangKy
                    .FirstOrDefaultAsync(p => p.MaPhieu == maPhieu && p.MaHoiVien == hoiVien.MaHoiVien);

                if (phieu == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy phiếu đăng ký hoặc bạn không có quyền hủy phiếu này!" });
                }

                if (phieu.TrangThai == "Đã hủy")
                {
                    return Json(new { success = false, message = "Phiếu đăng ký này đã được hủy trước đó!" });
                }

                // Cập nhật trạng thái thành Đã hủy
                phieu.TrangThai = "Đã hủy";
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = $"Đã hủy phiếu đăng ký {maPhieu} thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi hủy: " + ex.Message });
            }
        }

        // POST: /Home/CapNhatThongTin (Chức năng: Xem/cập nhật thông tin cá nhân)
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatThongTin([FromForm] CapNhatThongTinDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.HoTen))
            {
                return Json(new { success = false, message = "Họ và tên không được để trống!" });
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return Json(new { success = false, message = "Email không được để trống!" });
            }

            try
            {
                var taiKhoan = await GetCurrentTaiKhoanAsync();
                if (taiKhoan == null)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập lại!" });
                }

                var hoiVien = await GetOrCreateHoiVienAsync(taiKhoan);

                // Cập nhật TaiKhoan
                taiKhoan.HoTen = dto.HoTen.Trim();
                taiKhoan.Email = dto.Email.Trim();

                // Cập nhật HoiVien
                hoiVien.HoTen = dto.HoTen.Trim();
                hoiVien.Email = dto.Email.Trim();
                if (!string.IsNullOrWhiteSpace(dto.SoDienThoai))
                {
                    hoiVien.SoDienThoai = dto.SoDienThoai.Trim();
                }
                if (dto.NgaySinh.HasValue)
                {
                    hoiVien.NgaySinh = dto.NgaySinh.Value;
                }
                if (!string.IsNullOrWhiteSpace(dto.GioiTinh))
                {
                    hoiVien.GioiTinh = dto.GioiTinh;
                }

                _context.Update(taiKhoan);
                _context.Update(hoiVien);
                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "Cập nhật thông tin cá nhân thành công!",
                    hoTen = hoiVien.HoTen,
                    email = hoiVien.Email,
                    soDienThoai = hoiVien.SoDienThoai,
                    ngaySinh = hoiVien.NgaySinh.ToString("dd/MM/yyyy"),
                    gioiTinh = hoiVien.GioiTinh
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi cập nhật thông tin");
                return Json(new { success = false, message = "Lỗi khi lưu thông tin: " + ex.Message });
            }
        }

        // POST: /Home/DoiMatKhau (Chức năng: Đổi mật khẩu cá nhân)
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoiMatKhau([FromForm] DoiMatKhauDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.MatKhauCu) || string.IsNullOrWhiteSpace(dto.MatKhauMoi))
            {
                return Json(new { success = false, message = "Vui lòng nhập đầy đủ mật khẩu cũ và mới!" });
            }

            if (dto.MatKhauMoi.Length < 6 || dto.MatKhauMoi.Length > 12)
            {
                return Json(new { success = false, message = "Mật khẩu mới phải từ 6 đến 12 ký tự!" });
            }

            if (dto.MatKhauMoi != dto.XacNhanMatKhauMoi)
            {
                return Json(new { success = false, message = "Xác nhận mật khẩu mới không khớp!" });
            }

            try
            {
                var taiKhoan = await GetCurrentTaiKhoanAsync();
                if (taiKhoan == null)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập lại!" });
                }

                if (taiKhoan.MatKhau != dto.MatKhauCu)
                {
                    return Json(new { success = false, message = "Mật khẩu hiện tại không chính xác!" });
                }

                taiKhoan.MatKhau = dto.MatKhauMoi;
                _context.Update(taiKhoan);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Đổi mật khẩu thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi đổi mật khẩu: " + ex.Message });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Private Helpers

        private async Task<TaiKhoan?> GetCurrentTaiKhoanAsync()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated) return null;

            var usernameClaim = User.FindFirst("Username")?.Value ?? User.Identity.Name;
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(idClaim, out int maTaiKhoan))
            {
                var userById = await _context.TaiKhoan.FirstOrDefaultAsync(u => u.MaTaiKhoan == maTaiKhoan);
                if (userById != null) return userById;
            }

            if (!string.IsNullOrWhiteSpace(usernameClaim))
            {
                return await _context.TaiKhoan.FirstOrDefaultAsync(u => u.TenDangNhap == usernameClaim);
            }

            return null;
        }

        private async Task<HoiVien> GetOrCreateHoiVienAsync(TaiKhoan taiKhoan)
        {
            var hv = await _context.HoiVien
                .FirstOrDefaultAsync(h => h.MaTaiKhoan == taiKhoan.MaTaiKhoan.ToString());

            if (hv != null) return hv;

            // Tự động cấp mã hội viên mới nếu chưa có
            var allHVs = await _context.HoiVien.Select(h => h.MaHoiVien).ToListAsync();
            int nextNum = 1;
            foreach (var code in allHVs)
            {
                if (code.StartsWith("HV", StringComparison.OrdinalIgnoreCase) && int.TryParse(code[2..], out int n))
                {
                    if (n >= nextNum) nextNum = n + 1;
                }
            }

            hv = new HoiVien
            {
                MaHoiVien = $"HV{nextNum:D2}",
                MaTaiKhoan = taiKhoan.MaTaiKhoan.ToString(),
                HoTen = !string.IsNullOrWhiteSpace(taiKhoan.HoTen) ? taiKhoan.HoTen : taiKhoan.TenDangNhap,
                Email = taiKhoan.Email ?? "",
                SoDienThoai = "09" + new Random().Next(10000000, 99999999),
                GioiTinh = "Nam",
                NgaySinh = DateTime.Today.AddYears(-22),
                NgayThamGia = DateTime.Today,
                TrangThai = "Đang hoạt động"
            };

            _context.HoiVien.Add(hv);
            await _context.SaveChangesAsync();
            return hv;
        }

        private static int ParseMonths(string? thoiHanThang, string? tenGoi)
        {
            if (!string.IsNullOrWhiteSpace(thoiHanThang) && int.TryParse(thoiHanThang.Trim(), out int m))
            {
                return m;
            }

            if (!string.IsNullOrWhiteSpace(tenGoi))
            {
                if (tenGoi.Contains("12 Tháng", StringComparison.OrdinalIgnoreCase) || tenGoi.Contains("1 Năm", StringComparison.OrdinalIgnoreCase)) return 12;
                if (tenGoi.Contains("6 Tháng", StringComparison.OrdinalIgnoreCase)) return 6;
                if (tenGoi.Contains("3 Tháng", StringComparison.OrdinalIgnoreCase)) return 3;
                if (tenGoi.Contains("2 Tháng", StringComparison.OrdinalIgnoreCase)) return 2;
                if (tenGoi.Contains("1 Tháng", StringComparison.OrdinalIgnoreCase)) return 1;
            }

            return 1;
        }

        #endregion
    }
}
