using System.Collections.Generic;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    public class MemberDashboardViewModel
    {
        public List<GoiTap> DanhSachGoiTap { get; set; } = new List<GoiTap>();
        public List<PhieuDangKy> DanhSachPhieuDangKy { get; set; } = new List<PhieuDangKy>();
        public TaiKhoan? TaiKhoan { get; set; }
        public HoiVien? HoiVien { get; set; }
    }
}
