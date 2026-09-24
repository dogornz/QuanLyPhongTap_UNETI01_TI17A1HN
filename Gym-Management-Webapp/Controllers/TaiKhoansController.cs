
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gym_Management_Webapp.Models;

public class TaiKhoansController : Controller
{
    private readonly Gym_Management_WebappContext _context;

    public TaiKhoansController(Gym_Management_WebappContext context)
    {
        _context = context;
    }

    // GET: TAIKHOANS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.TaiKhoan.ToListAsync());
    }

    // GET: TAIKHOANS/Details/5
    public async Task<IActionResult> Details(int? mataikhoan)
    {
        if (mataikhoan == null)
        {
            return NotFound();
        }

        var taikhoan = await _context.TaiKhoan
            .FirstOrDefaultAsync(m => m.MaTaiKhoan == mataikhoan);
        if (taikhoan == null)
        {
            return NotFound();
        }

        return View(taikhoan);
    }

    // GET: TAIKHOANS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TAIKHOANS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MaTaiKhoan,TenDangNhap,MatKhau,HoTen,Email,VaiTro,TrangThai")] TaiKhoan taikhoan)
    {
        if (ModelState.IsValid)
        {
            _context.Add(taikhoan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(taikhoan);
    }

    // GET: TAIKHOANS/Edit/5
    public async Task<IActionResult> Edit(int? mataikhoan)
    {
        if (mataikhoan == null)
        {
            return NotFound();
        }

        var taikhoan = await _context.TaiKhoan.FindAsync(mataikhoan);
        if (taikhoan == null)
        {
            return NotFound();
        }
        return View(taikhoan);
    }

    // POST: TAIKHOANS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? mataikhoan, [Bind("MaTaiKhoan,TenDangNhap,MatKhau,HoTen,Email,VaiTro,TrangThai")] TaiKhoan taikhoan)
    {
        if (mataikhoan != taikhoan.MaTaiKhoan)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
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

    // GET: TAIKHOANS/Delete/5
    public async Task<IActionResult> Delete(int? mataikhoan)
    {
        if (mataikhoan == null)
        {
            return NotFound();
        }

        var taikhoan = await _context.TaiKhoan
            .FirstOrDefaultAsync(m => m.MaTaiKhoan == mataikhoan);
        if (taikhoan == null)
        {
            return NotFound();
        }

        return View(taikhoan);
    }

    // POST: TAIKHOANS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? mataikhoan)
    {
        var taikhoan = await _context.TaiKhoan.FindAsync(mataikhoan);
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
