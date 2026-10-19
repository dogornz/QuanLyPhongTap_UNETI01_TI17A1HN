using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    [Table("HoiVien")]
    public class HoiVien
    {
        [Key]
        [StringLength(450)]
        public string MaHoiVien { get; set; } = string.Empty;

        public string MaTaiKhoan { get; set; } = string.Empty;

        [Required]
        public string HoTen { get; set; } = string.Empty;

        public DateTime NgaySinh { get; set; } = DateTime.Today.AddYears(-22);

        public string GioiTinh { get; set; } = "Nam";

        public string SoDienThoai { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime NgayThamGia { get; set; } = DateTime.Now;

        public string TrangThai { get; set; } = "Đang hoạt động";
    }
}
