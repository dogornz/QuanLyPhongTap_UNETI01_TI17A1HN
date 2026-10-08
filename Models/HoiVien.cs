using System.ComponentModel.DataAnnotations;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    public class HoiVien
    {
        [Key]
        public int IdHoiVien { get; set; }
        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100, MinimumLength = 2)]
        public string HoTen { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(20, MinimumLength = 10)]
        public string SoDienThoai { get; set; }
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string DiaChi { get; set; }
        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        public DateTime NgaySinh { get; set; }
        [Required(ErrorMessage = "Giới tính không được để trống")]
        public string GioiTinh { get; set; }
        [Required]
        public string TrangThai { get; set; } //active, inactive, banned
        [Required]
        public DateTime NgayTao { get; set; }
        [Required]
        public DateTime NgayCapNhat { get; set; }
        public string GhiChu { get; set; }
        public string AnhDaiDien { get; set; }
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string TenDangNhap { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string MatKhau { get; set; }
        public ICollection<PhieuDangKy> PhieuDangKy { get; set; }
    }
}
