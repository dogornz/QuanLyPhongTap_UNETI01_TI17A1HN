using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    public class RegisterAccount
    {
        [Required]
        public string TenDangNhap { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; } = null!;

        [Required]
        public string HoTen { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }

    public class DangKyGoiTapDto
    {
        public string MaGoiTap { get; set; } = null!;
        public DateTime? NgayBatDau { get; set; }
    }

    public class CapNhatThongTinDto
    {
        public string HoTen { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string SoDienThoai { get; set; } = null!;
        public string DiaChi { get; set; } = null!;
        public DateTime? NgaySinh { get; set; }
        public string GioiTinh { get; set; } = null!;
    }

    public class DoiMatKhauDto
    {
        public string MatKhauCu { get; set; } = null!;
        public string MatKhauMoi { get; set; } = null!;
        public string XacNhanMatKhauMoi { get; set; } = null!;
    }
}
