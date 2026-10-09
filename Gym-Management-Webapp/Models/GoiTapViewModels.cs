// Họ và tên: [Điền họ và tên của bạn]
// Mã sinh viên: [Điền mã sinh viên của bạn]
// Nội dung thực hiện: Module 2 - ViewModel tìm kiếm, lọc, sắp xếp và phân trang danh sách gói tập
using System.Globalization;
using QuanLyPhongTap_UNETI01_TI17A1HN.Helpers;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.ViewModels
{
    public static class SapXepGoiTap
    {
        public const string TenAZ = "ten_asc";
        public const string TenZA = "ten_desc";
        public const string GiaTang = "gia_asc";
        public const string GiaGiam = "gia_desc";
        public const string ThoiHanTang = "thoihan_asc";
        public const string ThoiHanGiam = "thoihan_desc";

        public static readonly (string Ma, string Nhan)[] TatCa =
        {
            (TenAZ, "Tên gói A → Z"),
            (TenZA, "Tên gói Z → A"),
            (GiaTang, "Đơn giá tăng dần"),
            (GiaGiam, "Đơn giá giảm dần"),
            (ThoiHanTang, "Thời hạn tăng dần"),
            (ThoiHanGiam, "Thời hạn giảm dần")
        };

        public static bool HopLe(string? ma) => TatCa.Any(x => x.Ma == ma);
    }

    /// <summary>Các điều kiện người dùng gửi lên bằng query string (GET).</summary>
    public class GoiTapBoLoc
    {
        public string? TuKhoa { get; set; }
        public int? MaLoaiGoi { get; set; }
        public string? TrangThai { get; set; }
        public decimal? GiaTu { get; set; }
        public decimal? GiaDen { get; set; }
        public int? ThoiHanTu { get; set; }
        public int? ThoiHanDen { get; set; }
        public string? SapXep { get; set; }
        public int Trang { get; set; } = 1;

        /// <summary>
        /// Tạo route data cho link phân trang: GIỮ NGUYÊN từ khóa, bộ lọc, kiểu sắp xếp, chỉ đổi số trang.
        /// </summary>
        public Dictionary<string, string?> ToRouteData(int trang)
        {
            var d = new Dictionary<string, string?>();
            if (!string.IsNullOrWhiteSpace(TuKhoa)) d["TuKhoa"] = TuKhoa.Trim();
            if (MaLoaiGoi.HasValue) d["MaLoaiGoi"] = MaLoaiGoi.Value.ToString(CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(TrangThai)) d["TrangThai"] = TrangThai;
            if (GiaTu.HasValue) d["GiaTu"] = GiaTu.Value.ToString(CultureInfo.InvariantCulture);
            if (GiaDen.HasValue) d["GiaDen"] = GiaDen.Value.ToString(CultureInfo.InvariantCulture);
            if (ThoiHanTu.HasValue) d["ThoiHanTu"] = ThoiHanTu.Value.ToString(CultureInfo.InvariantCulture);
            if (ThoiHanDen.HasValue) d["ThoiHanDen"] = ThoiHanDen.Value.ToString(CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(SapXep)) d["SapXep"] = SapXep;
            d["Trang"] = Math.Max(1, trang).ToString(CultureInfo.InvariantCulture);
            return d;
        }
    }

    public class GoiTapIndexViewModel
    {
        public GoiTapBoLoc BoLoc { get; set; } = new();
        public PhanTrangDanhSach<GoiTap> KetQua { get; set; } = default!;
        public List<LoaiGoiTap> DanhSachLoaiGoi { get; set; } = new();
        public bool LaAdmin { get; set; }
    }
}
