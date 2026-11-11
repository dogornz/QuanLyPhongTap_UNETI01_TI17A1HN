using Microsoft.EntityFrameworkCore;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using QuanLyPhongTap_UNETI01_TI17A1HN.ViewModels;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Services
{
    public class QuanLyGoiTapService
    {
        private readonly QuanLyPhongTap_UNETI01_TI17A1HNContext _context;

        public QuanLyGoiTapService(QuanLyPhongTap_UNETI01_TI17A1HNContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Tạo ViewModel hợp nhất cho cả tab Gói tập (tìm kiếm, lọc, sắp xếp, phân trang) và tab Loại gói tập.
        /// </summary>
        public async Task<GoiTapVaLoaiIndexViewModel> BuildUnifiedIndexAsync(
            string? searchName,
            int? maLoaiGoi,
            string? trangThai,
            decimal? giaTu,
            decimal? giaDen,
            string? thoiHan,
            string? sortOrder,
            int page = 1,
            string activeTab = "goi")
        {
            var loaiGoiList = await _context.LoaiGoiTap
                .OrderBy(l => l.MaLoaiGoi)
                .ToListAsync();

            var packageCountsByCategory = await _context.GoiTap
                .Where(g => g.MaLoaiGoi.HasValue)
                .GroupBy(g => g.MaLoaiGoi!.Value)
                .ToDictionaryAsync(grp => grp.Key, grp => grp.Count());

            foreach (var item in loaiGoiList)
            {
                item.SoGoiTrucThuoc = packageCountsByCategory.TryGetValue(item.MaLoaiGoi, out var count)
                    ? count
                    : 0;
            }

            var query = _context.GoiTap
                .Include(g => g.LoaiGoiTap)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchName))
            {
                var kw = searchName.Trim().ToLower();
                query = query.Where(g => g.TenGoiTap.ToLower().Contains(kw));
            }

            if (maLoaiGoi.HasValue && maLoaiGoi > 0)
            {
                query = query.Where(g => g.MaLoaiGoi == maLoaiGoi.Value);
            }

            if (!string.IsNullOrWhiteSpace(trangThai) && trangThai != "all")
            {
                query = query.Where(g => g.TrangThai == trangThai);
            }

            if (giaTu.HasValue && giaTu > 0)
            {
                query = query.Where(g => g.DonGia >= giaTu.Value);
            }

            if (giaDen.HasValue && giaDen > 0)
            {
                query = query.Where(g => g.DonGia <= giaDen.Value);
            }

            if (!string.IsNullOrWhiteSpace(thoiHan) && thoiHan != "all")
            {
                query = query.Where(g => g.ThoiHanThang == thoiHan);
            }

            query = sortOrder switch
            {
                "ten_desc" => query.OrderByDescending(g => g.TenGoiTap),
                "gia_asc" => query.OrderBy(g => g.DonGia),
                "gia_desc" => query.OrderByDescending(g => g.DonGia),
                "thoihan_asc" => query.OrderBy(g => Convert.ToInt32(g.ThoiHanThang)),
                "thoihan_desc" => query.OrderByDescending(g => Convert.ToInt32(g.ThoiHanThang)),
                "ten_asc" => query.OrderBy(g => g.TenGoiTap),
                _ => query.OrderBy(g => g.TenGoiTap)
            };

            int totalItems = await query.CountAsync();
            const int pageSize = 5;
            int totalPages = Math.Max(1, (int)Math.Ceiling(totalItems / (double)pageSize));
            page = Math.Clamp(page, 1, totalPages);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var rawThoiHan = await _context.GoiTap
                .Select(g => g.ThoiHanThang)
                .Where(t => !string.IsNullOrEmpty(t))
                .Distinct()
                .ToListAsync();
            var thoiHanList = rawThoiHan
                .OrderBy(t => int.TryParse(t, out int val) ? val : 0)
                .ToList();

            return new GoiTapVaLoaiIndexViewModel
            {
                ActiveTab = string.Equals(activeTab, "loai", StringComparison.OrdinalIgnoreCase) ? "loai" : "goi",
                LoaiGoiTaps = loaiGoiList,
                GoiTapFilter = new GoiTapIndexViewModel
                {
                    Items = items,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    TotalItems = totalItems,
                    PageSize = pageSize,
                    SearchName = searchName,
                    MaLoaiGoi = maLoaiGoi,
                    TrangThai = trangThai,
                    GiaTu = giaTu,
                    GiaDen = giaDen,
                    ThoiHan = thoiHan,
                    SortOrder = sortOrder ?? "ten_asc",
                    LoaiGoiList = loaiGoiList,
                    ThoiHanList = thoiHanList
                }
            };
        }
    }
}

