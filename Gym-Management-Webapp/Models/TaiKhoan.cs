using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Gym_Management_Webapp.Models
{
    public class TaiKhoan
    {
        [Key]
        int MaTaiKhoan { get; set; }
        [StringLength(10), Required]
        string TenDangNhap { get; set; }
        [Range(6, 12), Required]
        string MatKhau { get; set; }
        [StringLength(50)]
        string HoTen { get; set; }
        [StringLength(50), EmailAddress]
        string Email { get; set; }
        [StringLength(50)]
        string VaiTro { get; set; }
        [StringLength(50)]
        string TrangThai { get; set; }
    }
}
