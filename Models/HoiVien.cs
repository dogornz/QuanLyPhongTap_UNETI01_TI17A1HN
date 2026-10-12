using System.ComponentModel.DataAnnotations;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    public class HoiVien
    {
        [Key]
        public int MaHoiVien { get; set; }
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

        [Required]
        public DateTime NgayThamGia { get; set; }
        [Required]
        public string TrangThai { get; set; } //active, inactive, banned
       
       
       
        
        public ICollection<PhieuDangKy> PhieuDangKy { get; set; }
    }
}
