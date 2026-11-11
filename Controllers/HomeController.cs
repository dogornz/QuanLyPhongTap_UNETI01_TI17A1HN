using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using QuanLyPhongTap_UNETI01_TI17A1HN.Services;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Controllers
{
    public class HomeController : Controller
    {
        private readonly QuanLyPhongTap_UNETI01_TI17A1HNContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IPasswordService _passwordService;
        private readonly PhieuDangKyService _phieuService;

        public HomeController(
            QuanLyPhongTap_UNETI01_TI17A1HNContext context,
            ILogger<HomeController> logger,
            IPasswordService passwordService,
            PhieuDangKyService phieuService)
        {
            _context = context;
            _logger = logger;
            _passwordService = passwordService;
            _phieuService = phieuService;
        }

        // GET: /Home/Index
        public async Task<IActionResult> Index()
        {
            var viewModel = new MemberDashboardViewModel();

            try
            {
                viewModel.DanhSachLoaiGoi = await _context.LoaiGoiTap
                    .Where(l => l.TrangThai == "Active")
                    .OrderBy(l => l.MaLoaiGoi)
                    .ToListAsync();

                viewModel.DanhSachGoiTap = await _context.GoiTap
                    .Include(g => g.LoaiGoiTap)
                    .Where(g => g.TrangThai == TrangThaiGoiTap.ApDung)
                    .OrderBy(g => g.MaGoiTap)
                    .ToListAsync();

                bool isAdmin = User.IsInRole("Admin") || User.IsInRole("admin");

                if (User.Identity != null && User.Identity.IsAuthenticated && !isAdmin)
                {
                    var taiKhoan = await GetCurrentTaiKhoanAsync();
                    if (taiKhoan != null)
                    {
                        viewModel.TaiKhoan = taiKhoan;
                        var hoiVien = await GetOrCreateHoiVienAsync(taiKhoan, allowCreate: true);
                        viewModel.HoiVien = hoiVien;

                        if (hoiVien != null)
                        {
                            viewModel.DanhSachPhieuDangKy = _phieuService
                                .GetByHoiVien(hoiVien.MaHoiVien)
                                .ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tải trang chủ hội viên");
            }

            return View(viewModel);
        }

        // POST: /Home/DangKyGoiTap (Ủy quyền qua PhieuDangKyService để đồng bộ quy tắc nghiệp vụ)
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangKyGoiTap([FromForm] DangKyGoiTapDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.MaGoiTap))
            {
                return Json(new { success = false, message = "Vui lòng chọn gói tập hợp lệ!" });
            }

            bool isAdmin = User.IsInRole("Admin") || User.IsInRole("admin");
            if (isAdmin)
            {
                return Json(new { success = false, message = "Tài khoản Quản trị viên vui lòng tạo phiếu tại mục Phiếu đăng ký để chọn hội viên!" });
            }

            try
            {
                var taiKhoan = await GetCurrentTaiKhoanAsync();
                if (taiKhoan == null)
                {
                    return Json(new { success = false, message = "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại!" });
                }

                var hoiVien = await GetOrCreateHoiVienAsync(taiKhoan, allowCreate: true);
                if (hoiVien == null)
                {
                    return Json(new { success = false, message = "Không thể xác định thông tin hội viên!" });
                }

                var goiTap = await _context.GoiTap.FirstOrDefaultAsync(g => g.MaGoiTap == dto.MaGoiTap);
                if (goiTap == null)
                {
                    return Json(new { success = false, message = "Gói tập không tồn tại trong hệ thống!" });
                }

                if (goiTap.TrangThai != TrangThaiGoiTap.ApDung)
                {
                    return Json(new { success = false, message = $"Gói tập '{goiTap.TenGoiTap}' hiện đang ở trạng thái '{goiTap.TrangThai}', không thể đăng ký!" });
                }

                DateTime ngayBatDau = dto.NgayBatDau.HasValue ? dto.NgayBatDau.Value.Date : DateTime.Today;
                var result = _phieuService.DangKy(
                    hoiVien.MaHoiVien,
                    goiTap.MaGoiTap,
                    ngayBatDau,
                    dto.XacNhanGoiKhac);

                return Json(new
                {
                    success = result.Success,
                    needConfirm = result.RequireConfirm,
                    tenGoiHienTai = result.TenGoiHienTai,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đăng ký gói tập");
                return Json(new { success = false, message = "Đã xảy ra lỗi trong quá trình đăng ký: " + ex.Message });
            }
        }

        // POST: /Home/HuyDangKy (Ủy quyền qua PhieuDangKyService.Huy để kiểm tra hết hạn)
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

                var hoiVien = await GetOrCreateHoiVienAsync(taiKhoan, allowCreate: false);
                var phieu = _phieuService.GetById(maPhieu);

                if (phieu == null || hoiVien == null || phieu.MaHoiVien != hoiVien.MaHoiVien)
                {
                    return Json(new { success = false, message = "Không tìm thấy phiếu đăng ký hoặc bạn không có quyền hủy phiếu này!" });
                }

                var result = _phieuService.Huy(maPhieu);
                return Json(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi hủy: " + ex.Message });
            }
        }

        // POST: /Home/CapNhatThongTin
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

                var hoiVien = await GetOrCreateHoiVienAsync(taiKhoan, allowCreate: true);
                if (hoiVien == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy hồ sơ hội viên!" });
                }

                taiKhoan.HoTen = dto.HoTen.Trim();
                taiKhoan.Email = dto.Email.Trim();

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

        // POST: /Home/DoiMatKhau
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoiMatKhau([FromForm] DoiMatKhauDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.MatKhauCu) || string.IsNullOrWhiteSpace(dto.MatKhauMoi))
            {
                return Json(new { success = false, message = "Vui lòng nhập đầy đủ mật khẩu cũ và mới!" });
            }

            if (dto.MatKhauMoi.Length < 6 || dto.MatKhauMoi.Length > 30)
            {
                return Json(new { success = false, message = "Mật khẩu mới phải từ 6 đến 30 ký tự!" });
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

                if (!_passwordService.VerifyPassword(taiKhoan.MatKhau, dto.MatKhauCu))
                {
                    return Json(new { success = false, message = "Mật khẩu hiện tại không chính xác!" });
                }

                taiKhoan.MatKhau = _passwordService.HashPassword(dto.MatKhauMoi);
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

        private async Task<HoiVien?> GetOrCreateHoiVienAsync(TaiKhoan taiKhoan, bool allowCreate)
        {
            var maTkStr = taiKhoan.MaTaiKhoan.ToString();
            var hv = await _context.HoiVien.FirstOrDefaultAsync(h => h.MaTaiKhoan == maTkStr);

            if (hv == null && !string.IsNullOrWhiteSpace(taiKhoan.Email))
            {
                var emailLower = taiKhoan.Email.Trim().ToLower();
                hv = await _context.HoiVien.FirstOrDefaultAsync(h => h.Email != null && h.Email.ToLower() == emailLower);
            }

            if (hv == null && !string.IsNullOrWhiteSpace(taiKhoan.TenDangNhap))
            {
                var codeLower = taiKhoan.TenDangNhap.Trim().ToLower();
                hv = await _context.HoiVien.FirstOrDefaultAsync(h => h.MaHoiVien.ToLower() == codeLower);
            }

            if (hv != null)
            {
                if (string.IsNullOrWhiteSpace(hv.MaTaiKhoan))
                {
                    hv.MaTaiKhoan = maTkStr;
                    _context.HoiVien.Update(hv);
                    await _context.SaveChangesAsync();
                }
                return hv;
            }

            if (!allowCreate)
            {
                return null;
            }

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
                MaTaiKhoan = maTkStr,
                HoTen = !string.IsNullOrWhiteSpace(taiKhoan.HoTen) ? taiKhoan.HoTen : taiKhoan.TenDangNhap,
                Email = taiKhoan.Email ?? string.Empty,
                SoDienThoai = "09" + Random.Shared.Next(10000000, 99999999),
                GioiTinh = "Nam",
                NgaySinh = DateTime.Today.AddYears(-22),
                NgayThamGia = DateTime.Today,
                TrangThai = TrangThaiHoiVien.HoatDong
            };

            _context.HoiVien.Add(hv);
            await _context.SaveChangesAsync();
            return hv;
        }

        #endregion
    }
}
