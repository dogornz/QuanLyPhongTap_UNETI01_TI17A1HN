namespace Quanlyphongtap.Models
{
    public class HoiVien
    {
        public int Id { get; set; }
        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string TrangThai { get; set; } //active, inactive, banned
        public DateTime NgayTao { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public string GhiChu { get; set; }
        public string AnhDaiDien { get; set; }
        public string MatKhau { get; set; }
        public ICollection<PhieuDangKy> PhieuDangKy { get; set; }
    }
}
