
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using QuanLyPhongTap_UNETI01_TI17A1HN.Services;

public class HoiViensController : Controller
{
    private readonly HoiVienService _service;

    public HoiViensController(
        HoiVienService service)
    {
        _service = service;
    }

    // =====================================
    // XEM DANH SÁCH
    // =====================================
    public IActionResult Index()
    {
        var danhSach = _service.GetAll();

        return View(danhSach);
    }

    // =====================================
    // TÌM KIẾM
    // =====================================
    [HttpGet]
    public IActionResult Search(string keyword)
    {
        var danhSach = _service.Search(keyword);

        ViewBag.Keyword = keyword;

        return View("Index", danhSach);
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
