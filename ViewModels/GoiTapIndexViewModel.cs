using QuanLyPhongTap_UNETI01_TI17A1HN.Models;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.ViewModels
{
    public class GoiTapIndexViewModel
    {
        public IEnumerable<GoiTap> Items { get; set; } = new List<GoiTap>();

        // 6.6. Phân trang
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int TotalItems { get; set; } = 0;
        public int PageSize { get; set; } = 5;

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        // 6.3. Tìm kiếm theo Tên gói tập (thực hiện bằng LINQ)
        public string? SearchName { get; set; }

        // 6.4. Lọc
        public int? MaLoaiGoi { get; set; }
        public string? TrangThai { get; set; }
        public decimal? GiaTu { get; set; }
        public decimal? GiaDen { get; set; }
        public string? ThoiHan { get; set; }

        // 6.5. Sắp xếp
        public string? SortOrder { get; set; }

        // Dữ liệu cho các Select lọc
        public IEnumerable<LoaiGoiTap>? LoaiGoiList { get; set; }
        public List<string>? ThoiHanList { get; set; }
    }
}

