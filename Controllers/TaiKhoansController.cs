using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using QuanLyPhongTap_UNETI01_TI17A1HN.Services;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Controllers
{
    [Authorize(Roles = "Admin,admin")]
    public class TaiKhoansController : Controller
    {
        private readonly QuanLyPhongTap_UNETI01_TI17A1HNContext _context;
        private readonly IPasswordService _passwordService;

        public TaiKhoansController(QuanLyPhongTap_UNETI01_TI17A1HNContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        // GET: TaiKhoans
        public async Task<IActionResult> Index()    
        {
            return View(await _context.TaiKhoan.ToListAsync());
        }

        // GET: TaiKhoans/Details/5
        public async Task<IActionResult> Details(int? id, int? mataikhoan)
        {
            var targetId = id ?? mataikhoan;

            if (targetId == null)
            {
                return NotFound();
            }
            
            var taikhoan = await _context.TaiKhoan
                .FirstOrDefaultAsync(m => m.MaTaiKhoan == targetId);

            if (taikhoan == null)
            {
                return NotFound();
            }

            return View(taikhoan);
        }

        // GET: TaiKhoans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TaiKhoans/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaTaiKhoan,TenDangNhap,MatKhau,HoTen,Email,VaiTro,TrangThai")] TaiKhoan taikhoan)
        {
            if (ModelState.IsValid)
            {
                // Băm mật khẩu bằng PBKDF2 HMAC-SHA256
                if (!string.IsNullOrWhiteSpace(taikhoan.MatKhau))
                {
                    taikhoan.MatKhau = _passwordService.HashPassword(taikhoan.MatKhau);
                }

                _context.Add(taikhoan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taikhoan);
        }

        // GET: TaiKhoans/Edit/5
        public async Task<IActionResult> Edit(int? id, int? mataikhoan)
        {
            var targetId = id ?? mataikhoan;

            if (targetId == null)
            {
                return NotFound();
            }

            var taikhoan = await _context.TaiKhoan.FindAsync(targetId);
            if (taikhoan == null)
            {
                return NotFound();
            }
            return View(taikhoan);
        }

        // POST: TaiKhoans/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, int? mataikhoan, [Bind("MaTaiKhoan,TenDangNhap,MatKhau,HoTen,Email,VaiTro,TrangThai")] TaiKhoan taikhoan)
        {
            var targetId = id ?? mataikhoan;
            if (targetId != taikhoan.MaTaiKhoan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.TaiKhoan.AsNoTracking().FirstOrDefaultAsync(t => t.MaTaiKhoan == taikhoan.MaTaiKhoan);
                    if (existing == null)
                    {
                        return NotFound();
                    }

                    // Nếu mật khẩu được nhập mới và chưa băm, tiến hành băm
                    if (!string.IsNullOrWhiteSpace(taikhoan.MatKhau) && taikhoan.MatKhau != existing.MatKhau && !_passwordService.IsHashed(taikhoan.MatKhau))
                    {
                        taikhoan.MatKhau = _passwordService.HashPassword(taikhoan.MatKhau);
                    }
                    else
                    {
                        // Giữ nguyên mật khẩu đã băm trước đó
                        taikhoan.MatKhau = existing.MatKhau;
                    }

                    _context.Update(taikhoan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaiKhoanExists(taikhoan.MaTaiKhoan))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(taikhoan);
        }

        // GET: TaiKhoans/Delete/5
        public async Task<IActionResult> Delete(int? id, int? mataikhoan)
        {
            var targetId = id ?? mataikhoan;
            if (targetId == null)
            {
                return NotFound();
            }

            var taikhoan = await _context.TaiKhoan
                .FirstOrDefaultAsync(m => m.MaTaiKhoan == targetId);
            if (taikhoan == null)
            {
                return NotFound();
            }

            return View(taikhoan);
        }

        // POST: TaiKhoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id, int? mataikhoan)
        {
            var targetId = id ?? mataikhoan;
            var taikhoan = await _context.TaiKhoan.FindAsync(targetId);
            if (taikhoan != null)
            {
                _context.TaiKhoan.Remove(taikhoan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaiKhoanExists(int? mataikhoan)
        {
            return _context.TaiKhoan.Any(e => e.MaTaiKhoan == mataikhoan);
        }
    }
}
