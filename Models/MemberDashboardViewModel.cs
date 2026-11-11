using QuanLyPhongTap_UNETI01_TI17A1HN.Services;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    public class MemberDashboardViewModel
    {
        public TaiKhoan? TaiKhoan { get; set; }
        public HoiVien? HoiVien { get; set; }
        public List<GoiTap> DanhSachGoiTap { get; set; } = new();
        public List<LoaiGoiTap> DanhSachLoaiGoi { get; set; } = new();
        public List<PhieuDangKy> DanhSachPhieuDangKy { get; set; } = new();

        // Statistics
        public int TongGoiDaDangKy => DanhSachPhieuDangKy.Count;
        public int SoGoiDangHieuLuc => DanhSachPhieuDangKy.Count(p =>
            p.TrangThai == TrangThaiPhieu.DangHieuLuc && p.NgayKetThuc.Date >= DateTime.Today);
        public int SoGoiChoXacNhan => DanhSachPhieuDangKy.Count(p => p.TrangThai == TrangThaiPhieu.ChoXacNhan);
        public decimal TongTienDaChi => DanhSachPhieuDangKy
            .Where(p => p.TrangThai == TrangThaiPhieu.DangHieuLuc)
            .Sum(p => p.TongTien > 0 ? p.TongTien : p.DonGia);

        // Active subscription info if available
        public PhieuDangKy? GoiHienTai => DanhSachPhieuDangKy
            .Where(p => p.TrangThai == TrangThaiPhieu.DangHieuLuc && p.NgayKetThuc.Date >= DateTime.Today)
            .OrderByDescending(p => p.NgayKetThuc)
            .FirstOrDefault();
    }

    public class DangKyGoiTapDto
    {
        public string MaGoiTap { get; set; } = string.Empty;
        public DateTime? NgayBatDau { get; set; }
        public bool XacNhanGoiKhac { get; set; }
        public string? PhuongThucThanhToan { get; set; }
        public string? GhiChu { get; set; }
    }

    public class CapNhatThongTinDto
    {
        public string HoTen { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public DateTime? NgaySinh { get; set; }
        public string GioiTinh { get; set; } = "Nam";
    }

    public class DoiMatKhauDto
    {
        public string MatKhauCu { get; set; } = string.Empty;
        public string MatKhauMoi { get; set; } = string.Empty;
        public string XacNhanMatKhauMoi { get; set; } = string.Empty;
    }
}
