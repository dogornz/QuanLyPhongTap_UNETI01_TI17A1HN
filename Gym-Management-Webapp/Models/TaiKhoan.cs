using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Gym_Management_Webapp.Models
{
    public class TaiKhoan
    {
        [Key]
        public int MaTaiKhoan { get; set; }
        [StringLength(10), Required]
        public string TenDangNhap { get; set; } = string.Empty;
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 50 ký tự"), Required]
        public string MatKhau { get; set; } = string.Empty;
        [StringLength(50)]
        public string HoTen { get; set; } = string.Empty;
        [StringLength(50), EmailAddress]
        public string Email { get; set; } = string.Empty;
        [StringLength(50)]
        public string VaiTro { get; set; } = string.Empty;
        [StringLength(50)]
        public string TrangThai { get; set; } = string.Empty;
    }
}
