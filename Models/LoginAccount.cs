using System.ComponentModel.DataAnnotations;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    public class LoginAccount
    {
        [Required]
        public string TenDangNhap { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; } = null!;

        public bool GhiNho { get; set; }

        public string? ReturnUrl { get; set; }
    }
}