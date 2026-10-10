using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    public class PhieuDangKy
    {
        [Key]
        public int MaPhieu { get; set; }
        [ForeignKey("MaHoiVien")] 
        public int MaHoiVien { get; set; }
        [ForeignKey("MaGoiTap")]
        public int MaGoiTap { get; set; }
        [Required]
        public DateTime NgayDangKy { get; set; }
        [Required]
        public DateTime NgayBatDau { get; set; }
        [Required]
        public DateTime NgayKetThuc { get; set; }
        [Required]
        public decimal DonGia { get; set; } //wait accept, existing, canceled 
        
        [Required(ErrorMessage = "Trạng thái không được để trống")]
        public string TrangThai { get; set; } // "Chờ xác nhận", "Đang hiệu lực", "Đã hủy"
        public HoiVien HoiVien { get; set; }
        public GoiTap GoiTap { get; set; }
    }
}
