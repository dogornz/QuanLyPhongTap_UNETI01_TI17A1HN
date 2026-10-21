using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    public class TaiKhoan
    {
        [Key]
        public int MaTaiKhoan { get; set; }
        [Required, StringLength(50), RegularExpression(@"^[a-zA-Z0-9]+$", 
            ErrorMessage = "Tên đăng nhập không được chứa ký tự đặc biệt !!!")]
        public string TenDangNhap { get; set; } = string.Empty;
        [Required, StringLength(12, MinimumLength = 6)]
        public string MatKhau { get; set; } = string.Empty;
        [StringLength(50, ErrorMessage = "Họ tên không được vượt quá 50 ký tự !!!")]
        public string? HoTen { get; set; }
        [Required, StringLength(50), EmailAddress (ErrorMessage = "Email không đúng định dạng !!!")]
        public string Email { get; set; } = string.Empty;
        public string VaiTro { get; set; } = "hoivien";
        public string TrangThai { get; set; } = "Unlock";

    }
}
