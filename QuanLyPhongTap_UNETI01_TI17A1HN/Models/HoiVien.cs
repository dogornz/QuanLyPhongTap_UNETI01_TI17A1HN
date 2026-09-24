using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    public class HoiVien
    {
        [Key]
        public string MaHoiVien { get; set; }
        [ForeignKey("TaiKhoan")]
        public string MaTaiKhoan { get; set; }
        [Required]
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string SoDienThoai { get; set; }
        public string Emnail { get; set; }
        public DateTime NgayThamGia { get; set; }
        public string TrangThai { get; set; }
    }
}
