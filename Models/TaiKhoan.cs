using System.ComponentModel.DataAnnotations;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    public class TaiKhoan
    {
        [Key]
        public int MaTaiKhoan { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Tên đăng nhập từ 5 đến 50 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Tên đăng nhập không được chứa ký tự đặc biệt !!!")]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        public string MatKhau { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự !!!")]
        public string? HoTen { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng !!!")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string VaiTro { get; set; } = "hoivien"; // "admin", "hoivien"

        [Required]
        public string TrangThai { get; set; } = "Unlock"; // "Lock", "Unlock"
    }
}
