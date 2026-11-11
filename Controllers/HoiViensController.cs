using System.Security.Claims;
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
    // XEM DANH SÁCH & LỌC (ADMIN)
    // =====================================
    [Authorize(Roles = "Admin,admin")]
    public IActionResult Index(string? keyword, string? trangThai, string? gioiTinh)
    {
        var danhSach = _service.Filter(keyword, trangThai, gioiTinh);

        ViewBag.Keyword = keyword;
        ViewBag.TrangThai = trangThai;
        ViewBag.GioiTinh = gioiTinh;
        return View(danhSach);
    }

    // =====================================
    // TÌM KIẾM
    // =====================================
    [HttpGet]
    [Authorize(Roles = "Admin,admin")]
    public IActionResult Search(string keyword)
    {
        return RedirectToAction(nameof(Index), new { keyword = keyword });
    }

    // =====================================
    // CHI TIẾT
    // =====================================
    [HttpGet]
    [Authorize(Roles = "Admin,admin")]
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
    [Authorize(Roles = "Admin,admin")]
    public IActionResult Create()
    {
        return View(new HoiVien { NgaySinh = DateTime.Today.AddYears(-20), TrangThai = "Đang hoạt động" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,admin")]
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
    [Authorize(Roles = "Admin,admin")]
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
    [Authorize(Roles = "Admin,admin")]
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
    [Authorize(Roles = "Admin,admin")]
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

        // Kiểm tra xem hội viên có còn thời gian gói tập không
        var activePackages = _service.GetActivePackages(id);
        bool hasActivePackage = activePackages.Any();

        ViewBag.HasActivePackage = hasActivePackage;
        ViewBag.ActivePackages = activePackages;
        ViewBag.CanDelete = !hasActivePackage;

        return View(hoiVien);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,admin")]
    public IActionResult DeleteConfirmed(string maHoiVien)
    {
        // Chặn admin xóa nếu hội viên vẫn còn thời gian tập
        if (_service.HasActivePackage(maHoiVien))
        {
            TempData["Error"] = "Hội viên còn thời gian tập! Không thể xóa hội viên này.";
            return RedirectToAction(nameof(Index));
        }

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
    [Authorize(Roles = "Admin,admin")]
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

    // =====================================
    // THÔNG TIN CÁ NHÂN
    // =====================================
    private HoiVien? GetCurrentHoiVien()
    {
        var maTaiKhoan = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var username = User.FindFirst("Username")?.Value ?? User.Identity?.Name;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var name = User.Identity?.Name;

        return _service.GetAll().FirstOrDefault(h =>
            (!string.IsNullOrEmpty(maTaiKhoan) && h.MaTaiKhoan == maTaiKhoan)
            || (!string.IsNullOrEmpty(username) && (h.MaHoiVien.Equals(username, StringComparison.OrdinalIgnoreCase) || (!string.IsNullOrEmpty(h.Email) && h.Email.Equals(username, StringComparison.OrdinalIgnoreCase))))
            || (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(h.Email) && h.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
            || (!string.IsNullOrEmpty(name) && (h.HoTen.Equals(name, StringComparison.OrdinalIgnoreCase) || h.MaHoiVien.Equals(name, StringComparison.OrdinalIgnoreCase)))
        );
    }

    [HttpGet]
    public IActionResult ThongTinCaNhan(string? id)
    {
        bool isAdmin = User.IsInRole("Admin") || User.IsInRole("admin");
        HoiVien? hoiVien = (isAdmin && !string.IsNullOrEmpty(id))
            ? _service.GetById(id)
            : GetCurrentHoiVien();

        if (hoiVien == null && isAdmin)
        {
            hoiVien = _service.GetAll().FirstOrDefault();
        }

        if (hoiVien == null)
        {
            return NotFound();
        }

        return View(hoiVien);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ThongTinCaNhan(HoiVien model)
    {
        bool isAdmin = User.IsInRole("Admin") || User.IsInRole("admin");
        if (!isAdmin)
        {
            var currentHoiVien = GetCurrentHoiVien();
            if (currentHoiVien == null || currentHoiVien.MaHoiVien != model.MaHoiVien)
            {
                TempData["Error"] = "Bạn không có quyền cập nhật thông tin hội viên khác!";
                return RedirectToAction(nameof(ThongTinCaNhan));
            }
        }

        if (ModelState.IsValid)
        {
            var res = _service.Update(model);
            if (res.success)
            {
                TempData["Success"] = "Cập nhật thông tin cá nhân thành công!";
                return View(model);
            }
            TempData["Error"] = res.message;
        }
        return View(model);
    }
}
