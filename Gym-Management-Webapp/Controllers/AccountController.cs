using System.Security.Claims;
using Gym_Management_Webapp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gym_Management_Webapp.Controllers
{
    public class AccountController : Controller
    {
        private readonly Gym_Management_WebappContext _context;

        public AccountController(Gym_Management_WebappContext context)
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
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
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

            if (!string.Equals(user.TrangThai, "Active", StringComparison.OrdinalIgnoreCase))
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

            if (string.Equals(user.VaiTro, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index", "Admin");
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

            return View(new RegisterViewModel());
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
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
                TrangThai = "Active"
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
