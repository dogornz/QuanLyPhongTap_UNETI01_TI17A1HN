
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quanlyphongtap.Models;

public class PhieuDangKiesController : Controller
{
    private readonly QuanlyphongtapContext _context;

    public PhieuDangKiesController(QuanlyphongtapContext context)
    {
        _context = context;
    }

    // GET: PHIEUDANGKYS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.PhieuDangKy.ToListAsync());
    }

    // GET: PHIEUDANGKYS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var phieudangky = await _context.PhieuDangKy
            .FirstOrDefaultAsync(m => m.Id == id);
        if (phieudangky == null)
        {
            return NotFound();
        }

        return View(phieudangky);
    }

    // GET: PHIEUDANGKYS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PHIEUDANGKYS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,IdHoiVien,IdGoiTap,NgayDangKy,NgayHetHan,GiaTriThanhToan,GiaTriGiaHan,TrangThai,HoiVien,GoiTap")] PhieuDangKy phieudangky)
    {
        if (ModelState.IsValid)
        {
            _context.Add(phieudangky);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(phieudangky);
    }

    // GET: PHIEUDANGKYS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var phieudangky = await _context.PhieuDangKy.FindAsync(id);
        if (phieudangky == null)
        {
            return NotFound();
        }
        return View(phieudangky);
    }

    // POST: PHIEUDANGKYS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,IdHoiVien,IdGoiTap,NgayDangKy,NgayHetHan,GiaTriThanhToan,GiaTriGiaHan,TrangThai,HoiVien,GoiTap")] PhieuDangKy phieudangky)
    {
        if (id != phieudangky.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(phieudangky);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhieuDangKyExists(phieudangky.Id))
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
        return View(phieudangky);
    }

    // GET: PHIEUDANGKYS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var phieudangky = await _context.PhieuDangKy
            .FirstOrDefaultAsync(m => m.Id == id);
        if (phieudangky == null)
        {
            return NotFound();
        }

        return View(phieudangky);
    }

    // POST: PHIEUDANGKYS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var phieudangky = await _context.PhieuDangKy.FindAsync(id);
        if (phieudangky != null)
        {
            _context.PhieuDangKy.Remove(phieudangky);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PhieuDangKyExists(int? id)
    {
        return _context.PhieuDangKy.Any(e => e.Id == id);
    }
}
