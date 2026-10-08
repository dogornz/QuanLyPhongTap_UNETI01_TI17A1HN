using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    public class PhieuDangKy
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("IdHoiVien")] 
        public int IdHoiVien { get; set; }
        [ForeignKey("IdGoiTap")]
        public int IdGoiTap { get; set; }
        [Required]
        public DateTime NgayDangKy { get; set; }
        [Required]
        public DateTime NgayHetHan { get; set; }
        [Required]
        public decimal GiaTriThanhToan { get; set; } //wait accept, existing, canceled 
        [Required]
        public decimal GiaTriGiaHan { get; set; }
        [Required(ErrorMessage = "Trạng thái không được để trống")]
        public string TrangThai { get; set; } // "Chờ xác nhận", "Đang hiệu lực", "Đã hủy"
        public HoiVien HoiVien { get; set; }
        public GoiTap GoiTap { get; set; }
    }
}
