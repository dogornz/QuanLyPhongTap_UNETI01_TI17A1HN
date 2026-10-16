using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using QuanLyPhongTap_UNETI01_TI17A1HN.Services;

[Authorize]
public class HoiViensController : Controller
{
    private readonly HoiVienService _service;

    public HoiViensController(HoiVienService service)
    {
        _service = service;
    }

    // =====================================
    // XEM DANH SÁCH
    // =====================================
    public IActionResult Index(string? keyword)
    {
        var danhSach = string.IsNullOrWhiteSpace(keyword) 
            ? _service.GetAll() 
            : _service.Search(keyword);

        ViewBag.Keyword = keyword;
        return View(danhSach);
    }

    // =====================================
    // TÌM KIẾM
    // =====================================
    [HttpGet]
    public IActionResult Search(string keyword)
    {
        return RedirectToAction(nameof(Index), new { keyword = keyword });
    }

    // =====================================
    // CHI TIẾT
    // =====================================
    [HttpGet]
    public IActionResult Details(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var hoiVien = _service.GetById(id);
        if (hoiVien == null)
        {
            return NotFound();
        }

        return View(hoiVien);
    }

    // =====================================
    // THÊM MỚI
    // =====================================
    [HttpGet]
    public IActionResult Create()
    {
        return View(new HoiVien { NgaySinh = DateTime.Today.AddYears(-20), TrangThai = "Đang hoạt động" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(HoiVien model)
    {
        if (ModelState.IsValid)
        {
            var res = _service.Add(model);
            if (res.success)
            {
                TempData["Success"] = res.message;
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, res.message);
        }
        return View(model);
    }

    // =====================================
    // CHỈNH SỬA
    // =====================================
    [HttpGet]
    public IActionResult Edit(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var hoiVien = _service.GetById(id);
        if (hoiVien == null)
        {
            return NotFound();
        }

        return View(hoiVien);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(HoiVien model)
    {
        if (ModelState.IsValid)
        {
            var res = _service.Update(model);
            if (res.success)
            {
                TempData["Success"] = res.message;
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, res.message);
        }
        return View(model);
    }

    // =====================================
    // XÓA
    // =====================================
    [HttpGet]
    public IActionResult Delete(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var hoiVien = _service.GetById(id);
        if (hoiVien == null)
        {
            return NotFound();
        }

        return View(hoiVien);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(string maHoiVien)
    {
        var res = _service.Delete(maHoiVien);
        if (res.success)
        {
            TempData["Success"] = res.message;
        }
        else
        {
            TempData["Error"] = res.message;
        }
        return RedirectToAction(nameof(Index));
    }

    // =====================================
    // CẬP NHẬT TRẠNG THÁI
    // =====================================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateTrangThai(
        string maHv,
        string trangThai)
    {
        var result = _service.UpdateTrangThai(
            maHv,
            trangThai);

        if (result.success)
        {
            TempData["Success"] =
                result.message;
        }
        else
        {
            TempData["Error"] =
                result.message;
        }

        return RedirectToAction(nameof(Index));
    }
}
