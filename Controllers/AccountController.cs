using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Controllers
{
    public class AccountController : Controller
    {
        private readonly QuanLyPhongTap_UNETI01_TI17A1HNContext _context;

        public AccountController(QuanLyPhongTap_UNETI01_TI17A1HNContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginAccount { ReturnUrl = returnUrl });
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginAccount model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.TaiKhoan
                .FirstOrDefaultAsync(u => u.TenDangNhap == model.TenDangNhap);

            if (user == null || user.MatKhau != model.MatKhau)
            {
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác.");
                return View(model);
            }

            if (!string.Equals(user.TrangThai, "Unlock", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(string.Empty, "Tài khoản của bạn đang bị khóa hoặc ngừng hoạt động.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.MaTaiKhoan.ToString()),
                new Claim(ClaimTypes.Name, !string.IsNullOrWhiteSpace(user.HoTen) ? user.HoTen : user.TenDangNhap),
                new Claim(ClaimTypes.Role, user.VaiTro ?? "User"),
                new Claim("Username", user.TenDangNhap),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.GhiNho,
                ExpiresUtc = model.GhiNho ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddHours(2)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            if (string.Equals(user.VaiTro, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index", "admin");
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new RegisterAccount());
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterAccount model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usernameExists = await _context.TaiKhoan
                .AnyAsync(u => u.TenDangNhap.ToLower() == model.TenDangNhap.ToLower());
            if (usernameExists)
            {
                ModelState.AddModelError("TenDangNhap", "Tên đăng nhập này đã được sử dụng.");
            }

            var emailExists = await _context.TaiKhoan
                .AnyAsync(u => u.Email.ToLower() == model.Email.ToLower());
            if (emailExists)
            {
                ModelState.AddModelError("Email", "Địa chỉ email này đã được đăng ký.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newAccount = new TaiKhoan
            {
                TenDangNhap = model.TenDangNhap.Trim(),
                MatKhau = model.MatKhau,
                HoTen = model.HoTen.Trim(),
                Email = model.Email.Trim(),
                VaiTro = "User",
                TrangThai = "Unlock"
            };

            _context.TaiKhoan.Add(newAccount);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đăng ký tài khoản thành công! Vui lòng đăng nhập để tiếp tục.";
            return RedirectToAction("Login");
        }

        // GET or POST: /Account/Logout
        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
