using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyPhongTap_UNETI01_TI17A1HN.DTOs;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using QuanLyPhongTap_UNETI01_TI17A1HN.Services;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly QuanLyPhongTap_UNETI01_TI17A1HNContext _context;
        private readonly JwtTokenService _jwtService;

        public AuthController(QuanLyPhongTap_UNETI01_TI17A1HNContext context, JwtTokenService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // POST: api/Auth/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taiKhoan = await _context.TaiKhoan
                .FirstOrDefaultAsync(tk => tk.TenDangNhap == model.TenDangNhap);

            if (taiKhoan == null)
            {
                return Unauthorized("Tên đăng nhập hoặc mật khẩu không đúng.");
            }

            var hasher = new PasswordHasher<TaiKhoan>();
            var result = hasher.VerifyHashedPassword(taiKhoan, taiKhoan.MatKhau, model.MatKhau);

            // Kiểm tra kết quả mã băm
            if (result == PasswordVerificationResult.Failed)
            {
                // Fallback nếu tài khoản cũ chưa kịp băm
                if (taiKhoan.MatKhau != model.MatKhau)
                {
                    return Unauthorized("Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }

            // Nếu mật khẩu cần rehash hoặc là chuỗi plain-text cũ, băm lại và lưu
            if (result == PasswordVerificationResult.SuccessRehashNeeded || taiKhoan.MatKhau == model.MatKhau)
            {
                taiKhoan.MatKhau = hasher.HashPassword(taiKhoan, model.MatKhau);
                await _context.SaveChangesAsync();
            }

            if (!string.Equals(taiKhoan.TrangThai, "Unlock", StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized("Tài khoản của bạn đang bị khóa hoặc ngừng hoạt động.");
            }

            // Tạo JWT và trả kết quả theo chuẩn bài giảng (Slide 31)
            string token = _jwtService.CreateToken(taiKhoan);

            return Ok(new
            {
                AccessToken = token,
                TokenType = "Bearer",
                VaiTro = taiKhoan.VaiTro,
                TenDangNhap = taiKhoan.TenDangNhap
            });
        }

        // GET: api/Auth/public
        [AllowAnonymous]
        [HttpGet("public")]
        public IActionResult PublicApi()
        {
            return Ok(new
            {
                Message = "API công khai, không cần đăng nhập."
            });
        }

        // GET: api/Auth/profile
        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            return Ok(new
            {
                Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                TenDangNhap = User.Identity?.Name,
                VaiTro = User.FindFirst(ClaimTypes.Role)?.Value
            });
        }

        // POST: api/Auth/create-user
        [Authorize(Roles = "Admin,admin")]
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string[] vaiTroHopLe = { "Admin", "admin", "hoivien", "HLV" };
            if (!vaiTroHopLe.Contains(model.VaiTro))
            {
                return BadRequest("Vai trò không hợp lệ.");
            }

            bool daTonTai = await _context.TaiKhoan
                .AnyAsync(tk => tk.TenDangNhap.ToLower() == model.TenDangNhap.ToLower());

            if (daTonTai)
            {
                return Conflict("Tên đăng nhập đã tồn tại.");
            }

            var taiKhoan = new TaiKhoan
            {
                TenDangNhap = model.TenDangNhap.Trim(),
                VaiTro = model.VaiTro,
                HoTen = model.HoTen?.Trim() ?? model.TenDangNhap.Trim(),
                Email = model.Email?.Trim() ?? $"{model.TenDangNhap}@gym.com",
                TrangThai = "Unlock"
            };

            var hasher = new PasswordHasher<TaiKhoan>();
            taiKhoan.MatKhau = hasher.HashPassword(taiKhoan, model.MatKhau);

            _context.TaiKhoan.Add(taiKhoan);
            await _context.SaveChangesAsync();

            return StatusCode(201, new
            {
                taiKhoan.MaTaiKhoan,
                taiKhoan.TenDangNhap,
                taiKhoan.VaiTro,
                taiKhoan.HoTen,
                taiKhoan.Email
            });
        }
    }
}

