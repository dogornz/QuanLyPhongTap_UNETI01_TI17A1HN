
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;

public class HoiViensController : Controller
{
    private readonly QuanLyPhongTap_UNETI01_TI17A1HNContext _context;

    public HoiViensController(QuanLyPhongTap_UNETI01_TI17A1HNContext context)
    {
        _context = context;
    }

    // GET: HOIVIENS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.HoiVien.ToListAsync());
    }

    // GET: HOIVIENS/Details/5
    public async Task<IActionResult> Details(string? mahoivien)
    {
        if (mahoivien == null)
        {
            return NotFound();
        }

        var hoivien = await _context.HoiVien
            .FirstOrDefaultAsync(m => m.MaHoiVien == mahoivien);
        if (hoivien == null)
        {
            return NotFound();
        }

        return View(hoivien);
    }

    // GET: HOIVIENS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: HOIVIENS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MaHoiVien,MaTaiKhoan,HoTen,NgaySinh,GioiTinh,SoDienThoai,Emnail,NgayThamGia,TrangThai")] HoiVien hoivien)
    {
        if (ModelState.IsValid)
        {
            _context.Add(hoivien);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(hoivien);
    }

    // GET: HOIVIENS/Edit/5
    public async Task<IActionResult> Edit(string? mahoivien)
    {
        if (mahoivien == null)
        {
            return NotFound();
        }

        var hoivien = await _context.HoiVien.FindAsync(mahoivien);
        if (hoivien == null)
        {
            return NotFound();
        }
        return View(hoivien);
    }

    // POST: HOIVIENS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? mahoivien, [Bind("MaHoiVien,MaTaiKhoan,HoTen,NgaySinh,GioiTinh,SoDienThoai,Emnail,NgayThamGia,TrangThai")] HoiVien hoivien)
    {
        if (mahoivien != hoivien.MaHoiVien)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(hoivien);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HoiVienExists(hoivien.MaHoiVien))
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
        return View(hoivien);
    }

    // GET: HOIVIENS/Delete/5
    public async Task<IActionResult> Delete(string? mahoivien)
    {
        if (mahoivien == null)
        {
            return NotFound();
        }

        var hoivien = await _context.HoiVien
            .FirstOrDefaultAsync(m => m.MaHoiVien == mahoivien);
        if (hoivien == null)
        {
            return NotFound();
        }

        return View(hoivien);
    }

    // POST: HOIVIENS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string? mahoivien)
    {
        var hoivien = await _context.HoiVien.FindAsync(mahoivien);
        if (hoivien != null)
        {
            _context.HoiVien.Remove(hoivien);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool HoiVienExists(string? mahoivien)
    {
        return _context.HoiVien.Any(e => e.MaHoiVien == mahoivien);
    }
}
